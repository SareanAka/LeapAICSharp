# Installation of Services and Dependencies

## Prerequisites

So you want to try out **LanguageLeapAI** huh? 
Do note that running WhisperAI and Voicevox on your local machine is not recommended as a significant amount of RAM and GPU power is required for the program to run efficiently.
If possible, you should run these 2 on another server either on your local network or on the cloud.


## Installing Dependencies

### Cloning this repository

Run this command to clone this entire repository.

```git clone https://github.com/SareanAka/LeapAICSharp```

Visual studio will automatically all the required NuGet packeges.

## Installing Services


### Docker

If you are using Google Colab notebooks to run Whisper AI and Voicevox, you can skip installing Docker Compose.

If you are planning on running the docker containers, you will have to ensure that you have Docker Compose installed.
You may follow these instructions [here](https://docs.docker.com/desktop/install/windows-install/) to install Docker Compose.

### Voicemeeter Banana

In order to proper route and separate audio between applications, your system audio, and python, we will be also be installing Voicemeeter Banana.
You can download and install Voicemeeter from their website [here](https://vb-audio.com/Voicemeeter/banana.htm).

After completing this step, you may move on to the next: Setting up [audio routing](AUDIO.md) using Voicemeeter Banana.
