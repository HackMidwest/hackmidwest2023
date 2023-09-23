import os
import azure.cognitiveservices.speech as speechsdk
import json

def text_to_speech_conv(text):
    # Keys to connect to azure resource
    with open('keys.json') as json_file:
        data = json.load(json_file)

    speech_key = data['speech_key']
    speech_origin = data['speech_origin']

    # This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
    speech_config = speechsdk.SpeechConfig(speech_key, speech_origin)
    audio_config = speechsdk.audio.AudioOutputConfig(use_default_speaker=True)

    # The language of the voice that speaks.
    speech_config.speech_synthesis_voice_name='en-US-JennyNeural'

    speech_synthesizer = speechsdk.SpeechSynthesizer(speech_config=speech_config, audio_config=audio_config)

    # Get text from the console and synthesize to the default speaker.
    # print("Enter some text that you want to speak >")
    # text = input()

    # Get the text the user prompted and converted into speech
    print(f"\t[TEXT TO SPEECHJ] Users asked: {text}")

    speech_synthesis_result = speech_synthesizer.speak_text_async(text).get()

    if speech_synthesis_result.reason == speechsdk.ResultReason.SynthesizingAudioCompleted:
        print("\t[TEXT TO SPEECHJ] Speech synthesized for text [{}]".format(text))
    elif speech_synthesis_result.reason == speechsdk.ResultReason.Canceled:
        cancellation_details = speech_synthesis_result.cancellation_details
        print("\t[TEXT TO SPEECHJ] Speech synthesis canceled: {}".format(cancellation_details.reason))
        if cancellation_details.reason == speechsdk.CancellationReason.Error:
            if cancellation_details.error_details:
                print("\t[TEXT TO SPEECHJ] Error details: {}".format(cancellation_details.error_details))
                print("\t[TEXT TO SPEECHJ] Did you set the speech resource key and region values?")
