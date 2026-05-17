namespace BattleBase.Localization
{
    public interface ILanguageVisitor
    {
        public void Visit(RuLanguage lang);

        public void Visit(EnLanguage lang);

        public void Visit(TrLanguage lang);
    }
}