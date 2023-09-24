import os
import azure.cognitiveservices.speech as speechsdk
import text_to_speech_conv
import speech_recognition
import openai
import json

# to run program, python3 main.py

with open('keys.json') as json_file:
    data = json.load(json_file)

openai.api_key = data['OAI-API']

api_call_counter = 0

intro_message = "Hello. How may I assist you?"
text_to_speech_conv.text_to_speech_conv(intro_message)


while True:
    user_text = speech_recognition.recognize_from_microphone()

    if user_text == "Stop.":
        print("[MAIN] User wants to end conversation")
        break
    else:
        # First, users speaks into mic asking a question. (wait for button click) (make a button indicting a question)
        # The users speech is converted into text
        # The users text is then sent to an AI and the AI responds with an answer in text
        # the AI text response is then converted into speech
        flag = input("type in y to get respond: ")
        if(flag == "y" or flag == "Y"):
            user_speech = text_to_speech_conv.text_to_speech_conv(user_text)

            response = openai.Completion.create(
                engine='text-davinci-003',
                prompt=user_text,
                max_tokens=1000
            )

            api_call_counter += 1

            ai_text = response.choices[0].text.strip()
            print(f"[MAIN] AI Test: {ai_text}")
            ai_speech = text_to_speech_conv.text_to_speech_conv(ai_text)

            print("[MAIN] Repeated the text from the user into audio. Looping to beginning...")


print(f"[MAIN] Number of AI API calls: {api_call_counter}")

goodbye_message = "[MAIN] You're welcome! Goodbye!"
text_to_speech_conv.text_to_speech_conv(goodbye_message)
