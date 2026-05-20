using BattleBase.Localization;

namespace BattleBase.Commands
{
    public sealed class CommandNextLanguage : CommandBase
    {
        public override void Execute() =>
            YandexGameLanguageSystemAdapter.Next();
    }
}