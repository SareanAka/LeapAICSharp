using Google.Protobuf;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace LeapAI.OpenAI
{
    
    public class OpenAIApi
    {
        private OpenAIAPI _api;
        private Conversation _conversation;


        public OpenAIApi(IniFileReader fileReader)
        {
            _api = new OpenAIAPI(fileReader.IniReadValue("AUTHENTICATION KEYS", "OPENAI_API_KEY"));
            _conversation = _api.Chat.CreateConversation();
            _conversation.Model = Model.ChatGPTTurbo;

            _conversation.AppendSystemMessage(File.ReadAllText("SystemInit.txt"));
            _conversation.AppendUserInput("You will be role-playing as Dorothy Franks, stay in your role at all times. NEVER EVER BREAK CHARACTER FOR ANY REASON");
        }

        public async Task<string> GetResponseAsync(string input)
        {
            _conversation.AppendUserInput(input);
            var response = await _conversation.GetResponseFromChatbot();

            var convo = _conversation.Messages.Where(x => !x.Role.Equals(ChatMessageRole.System));

            if (convo.Count() < 6) return response;
            _conversation.RequestParameters.Messages.RemoveAt(2);
            _conversation.RequestParameters.Messages.RemoveAt(2);

            return response;
        }
    }
}
