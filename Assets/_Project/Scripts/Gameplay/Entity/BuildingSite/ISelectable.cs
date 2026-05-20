namespace BattleBase.Gameplay
{
    public interface ISelectable
    {
        public bool TrySelect();

        public void Unselect();
    }
}