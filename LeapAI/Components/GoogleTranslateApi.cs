using System.Text.Json.Nodes;
using Newtonsoft.Json;
using RestSharp;

namespace LeapAI.Components
{
    public class GoogleTranslateApi
    {
        private readonly RestClient _client = new("https://clients5.google.com/translate_a/");


        public async Task<string?> GoogleTranslateAsync(string input)
        {
            try
            {
                var request = new RestRequest("single", Method.Post);
                request.AddParameter("client", "gtx");
                request.AddParameter("dt", "t");
                request.AddParameter("sl", "auto");
                request.AddParameter("tl", "ja");
                request.AddParameter("q", input);

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var json = response.Content;
                    
                    var jArray = JsonNode.Parse(json);
                    return jArray[0][0][0].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }

        private class GoogleTranslateResult
        {

        }
    }
}