namespace BattleBase.Localization
{
    public sealed class TrLanguage : ILanguage
    {
        public static readonly TrLanguage Instance = new();

        private TrLanguage() { }

        public void Accept(ILanguageVisitor visitor) =>
            visitor.Visit(this);
    }
}