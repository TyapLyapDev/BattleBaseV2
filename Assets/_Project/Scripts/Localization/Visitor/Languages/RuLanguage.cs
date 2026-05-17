namespace BattleBase.Localization
{
    public sealed class RuLanguage : ILanguage
    {
        public static readonly RuLanguage Instance = new();

        private RuLanguage() { }

        public void Accept(ILanguageVisitor visitor) =>
            visitor.Visit(this);
    }
}