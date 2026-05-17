namespace BattleBase.Localization
{
    public sealed class EnLanguage : ILanguage
    {
        public static readonly EnLanguage Instance = new();

        private EnLanguage() { }

        public void Accept(ILanguageVisitor visitor) =>
            visitor.Visit(this);
    }
}