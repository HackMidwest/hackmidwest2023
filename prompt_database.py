import os
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
import json


with open('keys.json') as json_file:
    data = json.load(json_file)

openai.api_key = data['OAI-API']
bolt_uri = data['neo4j-uri']
driver = GraphDatabase.driver(bolt_uri, auth=(data['neo4j-username'], data['neo4j-pass']))

def get_cypher(question):
    messages = [
        {"role": "system", "content": "You convert questions into Cypher queries for a Neo4j database"},
        {"role": "user", "content": "What was the score when Germany played Canada?"},
        {"role": "assistant", "content": "MATCH (:Team {name: 'Germany'})-[sg:PLAYED_IN]->(match:Match)<-[sc:PLAYED_IN]-(:Team {name: 'Canada'}) RETURN sg.score AS GermanyScore, sc.score AS CanadaScore"},
        {"role": "user", "content": "Which teams played against France?"},
        {"role": "assistant", "content": "MATCH (:Team {name: 'France'})-[:PLAYED_IN]->()<-[:PLAYED_IN]-(t:Team) RETURN DISTINCT t.name AS teamName"},
        {"role": "user", "content": "Which team scored the most goals against Sweden?"},
        {"role": "assistant", "content": "MATCH (:Team {name: 'Sweden'})-[:PLAYED_IN]->(match:Match)<-[s:PLAYED_IN]-(t:Team) RETURN t.name AS teamName, s.score AS score ORDER BY s.score DESC LIMIT 1"},
        {"role": "user", "content": question}]
    response = openai.ChatCompletion.create(
        model="gpt-4",
        messages=messages
    )
    query = response['choices'][0]['message']['content']
    return query

def get_neo4j_records(query):
    with driver.session() as session:
        result = session.run(query)
        records = [row.data() for row in result]
    return records

def format_answer(records, question):
    messages = [
        {"role": "system", "content": f"Use the JSON data provided by the user to answer to the question."},
        {"role": "user", "content": "Question: Which team scored the fewest goals against Japan? JSON: [{'teamName': 'Switzerland', 'score': 0}]"},
        {"role": "assistant", "content": "Switzerland scored the fewest goals against Japan."},
        {"role": "user", "content": "What were the scores when Japan played Sweden? JSON: [{'JapanScore': 3, 'SwedenScore': 1}, {'JapanScore': 0, 'SwedenScore': 8}, {'JapanScore': 0, 'SwedenScore': 2}]"},
        {"role": "assistant", "content": "The score when Japan played Sweden was as follows: \n- Japan: 3, Sweden: 1\n- Japan: 0, Sweden: 8,\n- Japan: 0, Sweden: 2"},
        {"role": "user", "content": f"{question} JSON:{json.dumps(records)}"}]
    response = openai.ChatCompletion.create(
        model="gpt-4",
        messages=messages
    )
    answer = response['choices'][0]['message']['content'] 
    return answer

def answer_question(question):
    query = get_cypher(question)
    print(f"Running cypher to get data.")
    print(query)
    records = get_neo4j_records(query)
    print(f"Found results.")
    print(records)
    answer = format_answer(records, question)
    return(answer)

# answer_question("What was the score when Japan played Sweden?")
