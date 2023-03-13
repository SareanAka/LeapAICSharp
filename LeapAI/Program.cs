using LeapAI.Components;
using NAudio.Wave;

namespace LeapAI;

public class Program
{
    // Instantiate the settings.ini file
    private static readonly IniFileReader FileReader = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini"));

    // Instantiate all the classes
    private static readonly AudioRecorder Recorder = new(FileReader);
    private static readonly WhisperApi Whisper = new (FileReader);
    private static readonly DeepLApi DeepL = new (FileReader);
    private static readonly GoogleTranslateApi GoogleTranslate = new();
    private static readonly VoiceVoxApi VoiceVox = new (FileReader);
    private static readonly AudioPlayer VoicePlayer = new(FileReader);

    // Get the keycode for the push-to-talk button
    private static readonly int VkN = int.Parse(FileReader.IniReadValue("PUSH TO TALK KEY", "MIC_RECORD_KEY"));

    public static async Task Main()
    {
        if (int.Parse(FileReader.IniReadValue("AUDIO DEVICE IDS", "MICROPHONE_ID")) == 99)
        {
            Console.WriteLine("Input Devices:");
            for (var i = -1; i < WaveInEvent.DeviceCount; i++)
            {
                var caps = WaveInEvent.GetCapabilities(i);
                Console.WriteLine($"ID: {i}, Device: {caps.ProductName}");
            }

            Console.WriteLine("\nOutput Devices:");
            var outputDeviceList = DirectSoundOut.Devices.ToList();
            for (var i = 0; i < outputDeviceList.Count; i++)
            {
                var caps = outputDeviceList[i];
                Console.WriteLine($"ID: {i - 1}, Device: {caps.Description}, GUID: {caps.Guid}");
            }
            Console.WriteLine("\n");
            Console.WriteLine("Press Crtl + c to close the program");

            while (true)
            {
                await Task.Delay(100);
            }
        }
        Console.WriteLine("Press your push-to-talk button to start recording\n");
        await CheckRecordAsync();
    }

    /// <summary>
    /// The core of the program, This will run all the code in the while loop.
    /// </summary>
    private static async Task CheckRecordAsync()
    {
        while (true)
        {
            // Check if the push-to-talk key is pressed
            if (AudioRecorder.GetAsyncKeyState(VkN) != 0)
            {
                // If not already recording, start recording
                if (!Recorder.IsRecording)
                {
                    Recorder.StartRecording();
                }
            }
            else
            {
                // If recording, stop recording
                if (Recorder.IsRecording)
                {
                    await Recorder.StopRecording();

                    // Transcribe the recording file
                    var transcribedText = await Whisper.TranscribeAsync("Audio/recording.wav");
                    if (!string.IsNullOrEmpty(transcribedText))
                    {
                        string? translatedText;
                        // Check if DeepL is in use
                        if (bool.Parse(FileReader.IniReadValue("TRANSLATOR", "USE_DEEPL")))
                        {
                            // Translate the transcribed text into Japanese
                            translatedText = await DeepL.TranslateAsync(transcribedText);
                        }
                        else
                        {
                            translatedText = await GoogleTranslate.GoogleTranslateAsync(transcribedText);
                        }

                        await File.WriteAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "translate.txt"), translatedText);

                        if (bool.Parse(FileReader.IniReadValue("LOGGING", "LOGGING")))
                        {
                            Console.WriteLine(transcribedText);
                            Console.WriteLine(translatedText);
                        }
                        
                        if (translatedText != null)
                        {
                            // Run the translated text into the Japanese Text-to-speech program
                            var result = await VoiceVox.TextToSpeechAsync(translatedText);

                            var attempts = 0;
                            while (result == null)
                            {
                                result = await VoiceVox.TextToSpeechAsync(translatedText);
                                attempts++;
                                if (attempts >= 5)
                                {
                                    break;
                                }
                                else
                                {
                                    await Task.Delay(100);
                                    Console.WriteLine("Retrying...");
                                }
                            }
                            if (result != null)
                            {
                                VoicePlayer.PlayAudio(result);
                            } else {Console.WriteLine("Could not get audio file.");}
                        } else {Console.WriteLine("No translated text was found.");}
                    } else {Console.WriteLine("Whisper has detected no speech.");}
                }
            }

            // Wait a bit to reduce CPU usage
            await Task.Delay(100);
        }
    }
}