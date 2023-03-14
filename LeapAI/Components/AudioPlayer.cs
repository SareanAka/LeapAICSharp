using NAudio.Wave;

namespace LeapAI.Components
{
    public class AudioPlayer
    {
        private DirectSoundOut outputDevice;

        public AudioPlayer(IniFileReader fileReader)
        {
            outputDevice = new DirectSoundOut(new Guid(fileReader.IniReadValue("AUDIO DEVICE IDS", "VOICEMEETER_INPUT_ID")));
        }

        public void PlayAudio(byte[] input)
        {
            var provider = new RawSourceWaveStream(
                new MemoryStream(input), new WaveFormat(rate: 44100, bits: 16, channels: 1));

            provider.Position += (int)Math.Floor(provider.WaveFormat.AverageBytesPerSecond * 0.1f);
            outputDevice.Stop();
            outputDevice.Init(provider);
            outputDevice.Play();
        }
    }
}