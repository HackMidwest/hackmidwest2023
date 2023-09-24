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

neo4j_pwd = "hackmidwest"

openai.api_key = openai_key

bolt_uri = "bolt://44.203.67.35:7687"
driver = GraphDatabase.driver(bolt_uri, auth=("Long", neo4j_pwd))

response = requests.get("https://archive.ics.uci.edu/static/public/450/sports+articles+for+objectivity+analysis.zip", stream=True)
with open("sports_articles.zip",'wb') as output:
    output.write(response.content)  

shutil.unpack_archive("sports_articles.zip", "sports_articles")

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

examples_df = examples_df._append(articles_df.loc[616])

articles_df.drop([0, 1, 616], inplace=True)

examples_df['entities'] = ""

print(examples_df.loc[0, "text"])



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

print(articles_df[0:10])

entities_df = pd.DataFrame(columns=["sports", "teams", "athletes", "events"])

for idx, row in articles_df[0:10].iterrows():
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