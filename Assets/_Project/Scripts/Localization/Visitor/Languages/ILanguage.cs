namespace BattleBase.Localization
{
    public interface ILanguage
    {
        public void Accept(ILanguageVisitor visitor);
    }
}