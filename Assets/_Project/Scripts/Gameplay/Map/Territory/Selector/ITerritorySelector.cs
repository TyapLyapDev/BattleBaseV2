using System;

namespace BattleBase.Gameplay.Map
{
    public interface ITerritorySelector
    {
        public event Action<Territory> Selected;
        public event Action<Territory> Unselected;

        public void Select(Territory territory);

        public void Unselect();
    }
}