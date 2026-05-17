using System;

namespace BattleBase.Gameplay.Map
{
    public class TerritorySelector : ITerritorySelector
    {
        private Territory _selectedTerritory;

        public event Action<Territory> Selected;
        public event Action<Territory> Unselected;

        public void Select(Territory territory)
        {
            if (territory == null)
                throw new ArgumentNullException(nameof(territory));

            if(territory == _selectedTerritory)
            {
                Unselect();

                return;
            }

            Unselect();
            _selectedTerritory = territory;

            Selected?.Invoke(territory);
        }

        public void Unselect()
        {
            Territory territory = _selectedTerritory;

            if (territory == null)
                return;

            _selectedTerritory = null;

            Unselected?.Invoke(territory);
        }
    }
}