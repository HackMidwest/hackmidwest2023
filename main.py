import os
import azure.cognitiveservices.speech as speechsdk
import text_to_speech_conv
import json

# Initialize the speech recognizer
r = sr.Recognizer()

while True:
    # Use microphone as the audio source
    with sr.Microphone() as source:
        print("Speak something...")
        audio = r.listen(source)

    try:
        # Convert speech to text
        text = r.recognize_google(audio)

        print("You said:", text)

        # Check if the user said "Thank you for your time"
        if "thank you for your time" in text.lower():
            break
        else:
            text_to_speech_conv.text_to_speech_conv(text)

    except sr.UnknownValueError:
        print("Sorry, I couldn't understand you.")

    except sr.RequestError:
        print("Sorry, I'm facing some technical issues.")

print("You're welcome! Goodbye!")