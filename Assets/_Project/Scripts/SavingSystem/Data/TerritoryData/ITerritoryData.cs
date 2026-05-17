using System.Collections.Generic;

namespace BattleBase.SaveService
{
    public interface ITerritoryData
    {
        public IReadOnlyList<int> ConqueredTerritories { get; }
    }
}