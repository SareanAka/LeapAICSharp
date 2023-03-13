using DeepL;

namespace LeapAI.Components
{
    public class DeepLApi
    {
        private static Translator _translator;
        public DeepLApi(IniFileReader fileReader)
        {
            _translator = new Translator(fileReader.IniReadValue("DEEPL AUTHENTICATION KEY", "DEEPL_AUTH_KEY"));
        }

        public async Task<string?> TranslateAsync(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var output = await _translator.TranslateTextAsync(input, null, LanguageCode.Japanese);
            return output.Text;
        }
    }
}
