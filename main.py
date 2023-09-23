import os
import azure.cognitiveservices.speech as speechsdk
import text_to_speech_conv
import speech_recognition

# Initialize the speech recognizer

intro_message = "Hello. How may I assist you?"
text_to_speech_conv.text_to_speech_conv(intro_message)

while True:
    # Use microphone as the audio source
    text = speech_recognition.recognize_from_microphone()

    if text == "Stop.":
        print("leaving")
        break
    else:
        text_to_speech_conv.text_to_speech_conv(text)

    # First, users speaks into mic asking a question. (wait for button click)
    # The users speech is converted into text
    # The users text is then sent to an AI and the AI responds with an answer in text
    # the AI text response is then converted into speech

goodbye_message = "You're welcome! Goodbye!"
text_to_speech_conv.text_to_speech_conv(goodbye_message)
