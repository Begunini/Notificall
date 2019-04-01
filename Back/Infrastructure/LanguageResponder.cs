using Database.Enums;

namespace Infrastructure
{
    public static class LanguageResponder
    {
        public static string DefineLanguage(string defaultLanguage, Language language)
        {
            var result = defaultLanguage;

            if (language == Language.Russian)
            {
                result = "ru-RU";
            }

            if (language == Language.English)
            {
                result = "en-US";
            }

            return result;
        }
    }
}
