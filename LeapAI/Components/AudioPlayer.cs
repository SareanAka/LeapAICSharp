using NAudio.Wave;

namespace LeapAI.Components
{
    public class AudioPlayer
    {
        private int outputDevice;
        private WaveOutEvent _waveOut;

        public AudioPlayer(IniFileReader fileReader)
        {
            outputDevice = int.Parse(fileReader.IniReadValue("AUDIO DEVICE IDS", "VOICEMEETER_INPUT_ID"));
            _waveOut = new WaveOutEvent
            {
                DeviceNumber = outputDevice,
                Volume = 1
            };
        }

        public void PlayAudio(byte[] input)
        {
            var provider = new RawSourceWaveStream(
                new MemoryStream(input), new WaveFormat(rate: 44100, bits: 16, channels: 1));

            _waveOut.Stop();
            _waveOut.Init(provider);
            _waveOut.Play();
        }
    }
}