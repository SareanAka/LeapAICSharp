﻿using DeepL;

namespace LeapAI.Components
{
    public class DeepLApi
    {
        private static Translator _translator;
        public DeepLApi(IniFileReader fileReader)
        {
            if (bool.Parse(fileReader.IniReadValue("TRANSLATOR", "USE_DEEPL")))
            {
                _translator = new Translator(fileReader.IniReadValue("AUTHENTICATION KEYS", "DEEPL_AUTH_KEY"));
            }
        }

        public async Task<string?> TranslateAsync(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var output = await _translator.TranslateTextAsync(input, null, LanguageCode.Japanese);
            return output.Text;
        }
    }
}
