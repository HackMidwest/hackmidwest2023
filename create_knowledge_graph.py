import requests
import shutil
from pathlib import Path
import pandas as pd
from neo4j import GraphDatabase
import openai
from getpass import getpass
import re
import time
import random
import backoff


openai_key = "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"

neo4j_pwd = "deposits-frequency-lifetimes"

openai.api_key = openai_key

bolt_uri = "bolt://44.203.67.35:7687"
driver = GraphDatabase.driver(bolt_uri, auth=("neo4j", neo4j_pwd))


p = Path('./sports_articles/Raw data')

articles = []
for d in p.iterdir():
    file_name = d.name
    try:
        with open(d, "rb") as f:
            text = f.read().decode("utf-8")
    except:
        with open(d, "rb") as f:
            text = f.read().decode("Windows-1252")
    articles.append({"article": file_name, "text": text})

articles_df = pd.DataFrame(articles)

articles_df.shape

examples_df = articles_df.iloc[:2,:]

articles_df.drop([0, 1], inplace=True)

examples_df['entities'] = ""

print(examples_df.loc[0, "text"])

examples_df.loc[0, "entities"] = "Sports:Ice dancing\nTeams:\nAthletes:Meryl Davis, Charlie, White, Judy Schwomeyer, James Sladky, Judy Blumberg, Michael Seibering, Namomi Lang, Peter Tchernyshev, Tanith Belbin, Ben Agosto, Madison Chock, Evan Bates, Maia Shibutani, Alex Shibutani, Marissa Castelli, Simon Shnapir, Tessa Virtue, Scott Moir\nSporting events:US Figure Skating Championships, Vancourver Olympics, 2010 World Figure Skating Championships, 2012 World Figure Skating Championships, 2011 World Figure Skating Championships, Figure Skating Grand Prix"

print(examples_df.loc[1, "text"])

examples_df.loc[1, "entities"] = "Sports:Soccer\nTeams:Bayern, Bayer Leverkusen, Frieburg, Borussia Dortmund\nAthletes:Christian Molinaro, Mario Mandzukic, Thomas Mueller\nSporting events:"

articles_df['len'] = articles_df['text'].str.len()

articles_df.describe()

ents_pattern = re.compile("Sports\:(.*)\nTeams\:(.*)\nAthletes\:(.*)\nSporting events\:(.*)", re.DOTALL)

@backoff.on_exception(backoff.expo, 
                      (openai.error.RateLimitError, 
                       openai.error.ServiceUnavailableError,
                       openai.error.APIError),
                     raise_on_giveup=False)
def get_entities(article):
    article_segments = [article[k:k+6000] for k in range(0, len(article), 6000)]
    sports, teams, athletes, events = [], [], [], []
    for segment in article_segments:
        messages = [
            {"role": "system", "content": "You extract entities in the following format:\nSports:<comma delimited list of strings>\nTeams:<comma delimited list of strings>\nAthletes:<comma delimited list of strings>\nSporting events:<comma delimited list of strings>"},
            {"role": "user", "content": examples_df.iloc[0, 1]},
            {"role": "assistant", "content": examples_df.iloc[0,2]},
            {"role": "user", "content": examples_df.iloc[1, 1]},
            {"role": "assistant", "content": examples_df.iloc[1,2]},
            {"role": "user", "content": examples_df.iloc[2,1]},
            {"role": "assistant", "content": examples_df.iloc[2,2]},
            {"role": "user", "content": segment}]
        retries = 0
        response = openai.ChatCompletion.create(
            model="gpt-3.5-turbo",
            messages=messages
            )
        result_match = ents_pattern.match(response['choices'][0]['message']['content'])
        if result_match:
            sports = sports + result_match.group(1).split(", ")
            teams = teams + result_match.group(2).split(", ")
            athletes = athletes + result_match.group(3).split(", ")
            events = events + result_match.group(4).split(", ")
        else:
            print(f"Result didn't match regex. {response}")
    entities = pd.Series({"sports": sports, "teams": teams, "athletes": athletes, "events": events})
    return entities



entities_df = pd.DataFrame(columns=["sports", "teams", "athletes", "events"])

for idx, row in articles_df.iterrows():
    entities = get_entities(row['text'])
    entities.name = idx
    entities_df = entities_df._append(entities)
    if entities_df.shape[0] % 25 == 0:
        print(f"Processed {entities_df.shape[0]} articles.")

output_df = articles_df.merge(entities_df, how="inner", left_index=True, right_index=True)


def send_row_to_neo4j(row):
    with driver.session() as session:
        session.run("""MERGE (a:Article {source: $article})
                       ON CREATE SET a.text = $text""", 
                    {"article": row["article"],
                     "text": row["text"]})
        session.run("""MATCH (a:Article {source: $article})
                       UNWIND $sports as sport
                       MERGE (s:Sport {name: toUpper(trim(sport))})
                       MERGE (a)-[:REFERENCES_SPORT]->(s)""",
                    {"article": row["article"],
                     "sports": row["sports"]})
        session.run("""MATCH (a:Article {source: $article})
                       UNWIND $athletes as athlete
                       MERGE (t:Athlete {name: toUpper(trim(athlete))})
                       MERGE (a)-[:REFERENCES_ATHLETE]->(t)""",
                    {"article": row["article"],
                     "athletes": row["athletes"]})
        session.run("""MATCH (a:Article {source: $article})
                       UNWIND $events as event
                       MERGE (e:Event {name: toUpper(trim(event))})
                       MERGE (a)-[:REFERENCES_EVENT]->(e)""",
                    {"article": row["article"],
                     "events": row["events"]})
                                                     
_ = output_df.apply(send_row_to_neo4j, axis=1)

with driver.session() as session:
    result = session.run("""MATCH (n) RETURN labels(n) as labels, count(*) as nodeCount""")
    result_df = pd.DataFrame([row.data() for row in result])
result_df



with driver.session() as session:
    result = session.run("""MATCH (s:Sport) 
                            RETURN s.name as sport, 
                            COUNT{ (s)<-[:REFERENCES_SPORT]-() } AS articleCount
                            ORDER BY articleCount DESC
                            LIMIT 10""")
    result_df = pd.DataFrame([row.data() for row in result])
result_df



with driver.session() as session:
    result = session.run("""MATCH (s1:Sport)<-[:REFERENCES_SPORT]-()-[:REFERENCES_SPORT]->(s2)
                            WHERE s1.name < s2.name
                            RETURN s1.name AS sport1, s2.name AS sport2, count(*) as articleCount
                            ORDER BY articleCount DESC
                            LIMIT 10""")
    result_df = pd.DataFrame([row.data() for row in result])
result_df

with driver.session() as session:
    result = session.run("""MATCH (a:Athlete) 
                            RETURN a.name as athlete, 
                            COUNT{ (a)<-[:REFERENCES_ATHLETE]-() } AS articleCount
                            ORDER BY articleCount DESC
                            LIMIT 10""")
    result_df = pd.DataFrame([row.data() for row in result])
result_df



with driver.session() as session:
    result = session.run("""MATCH (a:Athlete {name:"KOBE BRYANT"})-[:REFERENCES_ATHLETE]-(art)
                            MATCH (art)-[:REFERENCES_ATHLETE]->(n)
                            WHERE n <> a
                            RETURN n.name AS mentionedWithKobe, count(*) as articleCount
                            ORDER BY articleCount DESC
                            LIMIT 10""")
    result_df = pd.DataFrame([row.data() for row in result])
result_df



