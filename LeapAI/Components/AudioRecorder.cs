using NAudio.Wave;

using System.Runtime.InteropServices;

namespace LeapAI.Components
{

    public class AudioRecorder
    {
        public bool IsRecording;
        private WaveFileWriter? _writer;
        private readonly WaveInEvent _voiceInput;

        /// <summary>
        /// Constructor of the AudioRecorder class
        /// </summary>
        public AudioRecorder(IniFileReader fileReader)
        {
            // Settings of the recorder
            _voiceInput = new WaveInEvent
            {
                // Get microphone number from settings.ini file
                DeviceNumber = int.Parse(fileReader.IniReadValue("AUDIO DEVICE IDS", "MICROPHONE_ID")),
                WaveFormat = new WaveFormat(rate: 44100, bits: 16, channels: 1),
                BufferMilliseconds = 20
            };
            _voiceInput.DataAvailable += WaveIn_DataAvailable;
            _voiceInput.RecordingStopped += WaveSourceRecordingStopped;
        }
        

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public void StartRecording()
        {
            _writer = new WaveFileWriter("Audio/recording.wav", _voiceInput.WaveFormat);
            _voiceInput.StartRecording();
            IsRecording = true;
            Console.WriteLine("Recording started...");
        }

        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs eventArgs)
        {
            _writer?.Write(eventArgs.Buffer, 0, eventArgs.BytesRecorded);
            _writer?.Flush();
        }

        public async Task StopRecording()
        {
            _voiceInput.StopRecording();
            Console.WriteLine("Recording stopped...");
            while (IsRecording)
            {
                await Task.Delay(1);
            }
        }

        private void WaveSourceRecordingStopped(object? sender, StoppedEventArgs eventArgs)
        {
            _voiceInput.Dispose();
            _writer?.Dispose();
            IsRecording = false;
        }
    }
}
