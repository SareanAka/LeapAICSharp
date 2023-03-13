using NAudio.Wave;

namespace LeapAI.Components
{
    public class AudioPlayer
    {
        private DirectSoundOut outputDevice;

        public AudioPlayer(IniFileReader fileReader)
        {
            outputDevice = new DirectSoundOut(new Guid("df4af2ac-33ca-409d-b23f-306676c6c238"));
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