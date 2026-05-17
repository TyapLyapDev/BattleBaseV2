using BattleBase.Gameplay;
using BattleBase.Localization;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.UI
{
    [CreateAssetMenu(
        fileName = nameof(ProductionItemConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ProductionItemConfig))]
    public class ProductionItemConfig : ScriptableObject, IProductionItemInfo
    {
        [SerializeField] private Entity _prefab;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private LanguageTextsSet _name;
        [SerializeField] private LanguageTextsSet _description;
        [SerializeField] private int _price;

        public Entity Prefab => _prefab;

        public Sprite Sprite => _sprite;

        public LanguageTextsSet Name => _name;

        public LanguageTextsSet Description => _description;

        public int Price => _price;
    }
}