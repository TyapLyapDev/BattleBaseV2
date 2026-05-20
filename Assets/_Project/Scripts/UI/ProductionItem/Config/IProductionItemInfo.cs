using BattleBase.Gameplay;
using BattleBase.Localization;
using UnityEngine;

namespace BattleBase.UI
{
    public interface IProductionItemInfo
    {
        public Entity Prefab { get; }

        public Sprite Sprite { get; }

        public LanguageTextsSet Name { get; }

        public LanguageTextsSet Description { get; }

        public int Price { get; }
    }
}