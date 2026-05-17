namespace BattleBase.Gameplay
{
    public interface IBuildingSiteSelector
    {
        public bool TrySelect(IBuildingSite site);

        public void Unselect();
    }
}