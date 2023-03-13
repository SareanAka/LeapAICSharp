# Writing your .env file

First, copy [settings.ini](LeapAI/settingsPreview.ini) to .env by running the following command.

```copy settingsPreview.ini settings.ini```

Now open settings.ini in a text editor of your choice and update the variables. Below is a more detailed description of each environment variable

Important is that the settings.ini file should be copied to the output directory, make sure this is set to "always" or "copy if newer"

## Logging 

This variable can be set to either _True_ or _False_. Set to _True_ if you would like to see more detailed logging from the terminal when running the console program.
Set to _False_ if you want to disable logging.

## Services Urls 

These are the base urls for the Whisper and Voicevox services. You can leave it as localhost if you are running these on your local machine.
If you are running them using Google Colab or on a different port number, be sure to update these variables with the appropriate urls and ports.

## DeepL Authentication Key

The DEEPL_AUTH_KEY variable where you paste your DeepL authentication key. Sign up for a free plan [here](https://www.deepl.com/pro-api?cta=header-pro-api).
Then go to this [link](https://www.deepl.com/account/summary), scroll down to the `Authentication Key for DeepL API` section to copy your API key.

The TARGET_LANGUAGE_CODE variable is where you paste the language code of your desired language to translate. 
Use [this website](https://www.andiamo.co.uk/resources/iso-language-codes) to select the correct language code according to ISO 639-1 


## Push to talk key

The keyCode to hold down when you want your voice to be recorded and translated. E.g. MIC_RECORD_KEY=78 if you want to hold down the 'n' key.
You can find the keycode for your desired key [here](https://www.toptal.com/developers/keycode)

## Audio Device Ids

Here is where you will enter the IDs for the various audio devices that the program will be using.
This is required for C# to know which audio device to listen from or play audio to.
When you run the app for the first time (or if MICROPHONE_ID=99) the console will display your output devices, here you can obtain the ID or GUID for your desired output device.

## Voicevox Settings

Choose which speaker to use from Voicevox by updating VOICE_ID. 
Send a curl request to get a list of all speaker IDs and their corresponding speakers.
Replace <VOICEVOX_BASE_URL> with the url that Voicevox is hosted at.

```curl <VOICEVOX_BASE_URL>/speakers```

Feel free to adjust the scaling of the speaker's volume, speed or intonation as well.

## Finish

You are finally done setting up your settings.ini variables. To start running **LanguageLeapAI**, go to [usage](../README.md#Usage).
