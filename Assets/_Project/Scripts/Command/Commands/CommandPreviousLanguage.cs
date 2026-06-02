using BattleBase.Localization;

namespace BattleBase.Commands
{
    public sealed class CommandPreviousLanguage : CommandBase
    {
        public override void Execute() =>
            YandexGameLanguageSystemAdapter.Previous();
    }
}