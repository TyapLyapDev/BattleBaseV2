using BattleBase.Localization;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    [CreateAssetMenu(
        fileName = nameof(TerritoryConfig), 
        menuName = Constants.ConfigsAssetMenuPath + nameof(TerritoryConfig))]
    public class TerritoryConfig : ScriptableObject, ITerritoryInfo
    {
        [SerializeField] private LanguageTextsSet _territoryName;

        public LanguageTextsSet TerritoryName => _territoryName;
    }
}