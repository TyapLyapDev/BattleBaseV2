using BattleBase.Localization;

namespace BattleBase.Commands
{
    public sealed class CommandNextLanguage : CommandBase
    {
        protected override void OnExecute() =>
            YandexGameLanguageSystemAdapter.Next();
    }
}