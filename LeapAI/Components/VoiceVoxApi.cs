using RestSharp;
using Newtonsoft.Json;
using System.Globalization;

namespace LeapAI.Components
{
    public class VoiceVoxApi
    {
        private readonly RestClient _client;
        private readonly string _speakerId;

        private readonly float _speedScale;
        private readonly float _volumeScale;
        private readonly float _intonationScale;
        private readonly float _prePhonemeLength;
        private readonly float _postPhonemeLength;

        /// <summary>
        /// Initialize all the settings for VoiceVox
        /// </summary>
        public VoiceVoxApi(IniFileReader fileReader)
        {
            _client = new RestClient(fileReader.IniReadValue("SERVICES URLS", "VOICEVOX_BASE_URL"));
            _speakerId = fileReader.IniReadValue("VOICEVOX SETTINGS", "VOICE_ID");
            _speedScale = float.Parse(fileReader.IniReadValue("VOICEVOX SETTINGS", "SPEED_SCALE"), CultureInfo.InvariantCulture);
            _volumeScale = float.Parse(fileReader.IniReadValue("VOICEVOX SETTINGS", "VOLUME_SCALE"), CultureInfo.InvariantCulture);
            _intonationScale = float.Parse(fileReader.IniReadValue("VOICEVOX SETTINGS", "INTONATION_SCALE"), CultureInfo.InvariantCulture);
            _prePhonemeLength = float.Parse(fileReader.IniReadValue("VOICEVOX SETTINGS", "PRE_PHONEME_LENGTH"), CultureInfo.InvariantCulture);
            _postPhonemeLength = float.Parse(fileReader.IniReadValue("VOICEVOX SETTINGS", "POST_PHONEME_LENGTH"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// This task will convert the translated text into a request to the voicevox API and return a sound byte array.
        /// </summary>
        /// <param name="input">Input the text that was translated into Japanese</param>
        /// <returns>A byte array that can be written as a .wav sound file</returns>
        public async Task<byte[]?> TextToSpeechAsync(string? input)
        {
            var audioQueryRequest = new RestRequest("audio_query", Method.Post);
            audioQueryRequest.AddQueryParameter("text", input);
            audioQueryRequest.AddQueryParameter("speaker", _speakerId);

            var audioQueryResponse = await _client.ExecuteAsync(audioQueryRequest);

            if (!audioQueryResponse.IsSuccessful) return null;
            if (audioQueryResponse.Content == null) return null;

            var result = JsonConvert.DeserializeObject<AudioQueryResult>(audioQueryResponse.Content);

            if (result == null) return null;

            result.speedScale = _speedScale;
            result.volumeScale = _volumeScale;
            result.intonationScale = _intonationScale;
            result.prePhonemeLength = _prePhonemeLength;
            result.postPhonemeLength = _postPhonemeLength;

            var synthesisRequest = new RestRequest("synthesis", Method.Post);
            synthesisRequest.AddQueryParameter("speaker", _speakerId);
            synthesisRequest.AddJsonBody(result);

            return await _client.DownloadDataAsync(synthesisRequest);
        }

        #region JsonClasses
        private class AudioQueryResult
        {
            public Accent_Phrases[] accent_phrases { get; set; }
            public float speedScale { get; set; }
            public float pitchScale { get; set; }
            public float intonationScale { get; set; }
            public float volumeScale { get; set; }
            public float prePhonemeLength { get; set; }
            public float postPhonemeLength { get; set; }
            public int outputSamplingRate { get; set; }
            public bool outputStereo { get; set; }
            public string kana { get; set; }
        }

        private class Accent_Phrases
        {
            public Mora[] moras { get; set; }
            public int accent { get; set; }
            public object pause_mora { get; set; }
            public bool is_interrogative { get; set; }
        }

        private class Mora
        {
            public string text { get; set; }
            public string consonant { get; set; }
            public float? consonant_length { get; set; }
            public string vowel { get; set; }
            public float vowel_length { get; set; }
            public float pitch { get; set; }
        }
        #endregion
    }
}
