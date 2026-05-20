namespace BattleBase.Gameplay
{
    public interface IBuildingSiteSelector
    {
        public bool TrySelect(ISelectable site);

        public void Unselect();
    }
}