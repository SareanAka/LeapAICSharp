using RestSharp;

namespace LeapAI.Components
{
    public class WhisperApi
    {
        private readonly RestClient _client;

        /// <summary>
        /// Constructor of the AudioRecorder class
        /// </summary>
        public WhisperApi(IniFileReader fileReader)
        {
            _client = new RestClient(fileReader.IniReadValue("SERVICES URLS", "WHISPER_BASE_URL"));
        }

        /// <summary>
        /// Asynchronous method that posts a request to the WhisperAPI and returns a string of recognized audio
        /// </summary>
        /// <param name="filePath">file path to the audio file</param>
        /// <returns></returns>
        public async Task<string?> TranscribeAsync(string filePath)
        {
            try
            {
                var fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                if (File.Exists(fullFilePath))
                {
                    var request = new RestRequest("asr", Method.Post);
                    request.AddParameter("task", "transcribe");
                    request.AddParameter("language", "en");
                    request.AddParameter("output", "json");
                    request.AddFile("audio_file", fullFilePath);

                    var response = await _client.ExecuteAsync(request);

                    if (response.IsSuccessful)
                    {
                        var text = response.Content?.Trim();
                        return text;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
    }
}
