namespace BattleBase.SaveService
{
    public interface ITerritorySaver
    {
        public ITerritoryData TerritoryData { get; }

        public void SetTerritoryData(ITerritoryData data);
    }
}