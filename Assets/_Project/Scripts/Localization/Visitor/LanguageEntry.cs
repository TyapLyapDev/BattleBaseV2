namespace BattleBase.Localization
{
    public sealed class LanguageEntry
    {
        public LanguageEntry(ILanguage language, string code)
        {
            Language = language;
            Code = code;
        }

        public ILanguage Language { get; }

        public string Code { get; }
    }
}