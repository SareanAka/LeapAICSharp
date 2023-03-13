# Notice

This project is a CSharp rewrite of an existing project by the content creator [SociallyIneptWeep](https://www.youtube.com/@sociallyineptweeb).
I made this in CSharp both to optimize the code in my own way and also learn more about the language. Also because I was bored starved for a female voi-
Anyways...
Most of the documentation here was written by [SociallyIneptWeep](https://www.youtube.com/@sociallyineptweeb) and can be found in the [original repository](https://github.com/SociallyIneptWeeb/LanguageLeapAI), I merely rewrote some of it to fit my application. Go show him some love!

# LeapAICSharp

LEAP across Language barriers by using AI to converse with other online users from across the globe!
**LanguageLeapAI** aims to provide you a real-time language AI assistant that can understand and speak your desired language fluently. 
*(Targeted towards English to Japanese as of right now)*


## Integration of AI Entities

This project integrates 3 free and open-source AI systems:

1. [WhisperAI](https://github.com/openai/whisper): General-purpose Speech Recognition Model developed by OpenAI that can perform multilingual speech
recognition.
2. [DeepL Translator](https://www.deepl.com/translator): Powered by neural networks and the latest AI innovations for natural-sounding translations
3. [Voicevox](https://voicevox.hiroshiba.jp/): Japanese Deep-Learning AI Voice Synthesizer


WhisperAI and Voicevox both have docker images available on DockerHub, so we will be building and running them both via a [Docker Compose file](Docker&Colab/docker-compose.yml).
DeepL can be interacted with by signing up for a free plan and interacting with its [REST API](https://www.deepl.com/pro-api?cta=header-pro-api/) up to 500,000 character limit / month


### How the Voice Translator works

When run this application will record your microphone whenever a push-to-talk key is held down on the keyboard.
Once this key is released, it saves your voice in an audio file which is then sent to the whisperApi class which will post a request to WhisperAI which runs Automatic Speech Recognition (ASR) on it.
WhisperApi will return the text which was transcribed which is sent to the translator that is currently in use (Google Translate by default and DeepL when USE_DEEPL=true in the settings.ini file)

The translated text is then sent to Voicevox which performs text-to-speech and generates an audio file voiced in Japanese.
This file is then played to your target application's microphone input and your speakers/headphones.

Since Voicevox only takes in Japanese text as input and generates speech in Japanese, the project is technically only limited to Japanese as the target language.
However, Voicevox can be replaced with any other text to speech program that can speak your desired language for limitless possibilities.

## Setup

Setting up **LanguageLeapAI** requires 3 crucial steps, so don't miss out on any of them!
1. [Installing Services and Dependencies](docs/INSTALLATION.md)
2. [Audio Routing](docs/AUDIO.md)
3. [Writing your settings.ini file](docs/ENV.md)


## Usage

To run **LanguageLeapAI**, you need to first run WhisperAI and Voicevox. They can either be run via Docker or using Google Colab.

### Google Colab

If your GPU is not powerful enough, you may want to consider running WhisperAI and Voicevox using Google Colab's GPU.

Upload [run_whisper_colab.ipynb](Docker&Colab/run_whisper_colab.ipynb) and [run_voicevox_colab.ipynb](Docker&Colab/run_voicevox_colab.ipynb) files to Google drive, open the notebook with Google Colab and simply follow the instructions!

### Docker

If you still want to run both Whisper and Voicevox on your computer, run these commands in the folder containing the [docker-compose.yml](docker-compose.yml) file.

To run both WhisperAI and Voicevox:

```docker-compose up -d```

To stop running the containers:

```docker-compose down```


### Things to note

Some important things to keep in mind while using **LanguageLeapAI**.

#### Whisper's inconsistency

Do note that WhisperAI is not exactly the most accurate and will not transcribe speech correctly 100% of the time, so use at your own risk.
Until OpenAI decides to improve the dataset that was used to train the Whisper models, this will have to do.

Also, Whisper is not designed to handle multiple concurrent requests at once.
However, for subtitles to be updated in time, multiple requests are being sent asynchronously, so some requests might return an error.

#### Antivirus Web Protection

If you are running Whisper and Voicevox on the cloud using Google Colab, since we are using ngrok and localtunnel to host our services,
the randomised public IP address that they provide might be blacklisted by your antivirus software. If the AI seems to stop working,
it may be due to your antivirus blocking the connections to these public IP addresses. 
You may whitelist these IP addresses or just turn off your antivirus web protection **at your own risk**.

#### Voicevox voices

There are certain terms and conditions for using the voices from Voicevox, so do read up on [these](https://voicevox.hiroshiba.jp/) before using a specific speaker.


#### Application limitations

Some applications like Valorant for some reason does not allow open mic for team voice chat, so **LanguageLeapAI** will not work for in these cases,
unless you hold down the push to talk button whenever you want your teammates to hear the Text-to-Speech.
However, Valorant does have open mic for party voice-chat, so there should be no issue if it's used towards your party members.

## License

The code of LanguageLeapAI is released under the MIT License. See [LICENSE](LICENSE) for further details.
