using System.Collections.Generic;

namespace BattleBase.SaveService
{
    public interface ITerritoryData : IChangeTrackable<ITerritoryData>
    {
        public IReadOnlyList<int> ConqueredTerritories { get; }
    }
}