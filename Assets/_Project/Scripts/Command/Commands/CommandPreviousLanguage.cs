using BattleBase.Localization;

namespace BattleBase.Commands
{
    public sealed class CommandPreviousLanguage : CommandBase
    {
        protected override void OnExecute() =>
            YandexGameLanguageSystemAdapter.Previous(); 
    }
}