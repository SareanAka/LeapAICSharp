using LeapAI.Components;

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

    // Get the keycode for the push-to-talk button
    private static readonly int VkN = int.Parse(FileReader.IniReadValue("PUSH TO TALK KEY", "MIC_RECORD_KEY"));

    public static async Task Main()
    {
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

                            await using var writer = new BinaryWriter(File.OpenWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", "resultJP.wav")));
                            if (result != null) writer.Write(result);
                        } else {Console.WriteLine("No translated text was found.");}
                    } else {Console.WriteLine("Whisper has detected no speech.");}
                }
            }

            // Wait a bit to reduce CPU usage
            await Task.Delay(100);
        }
    }
}