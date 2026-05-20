namespace BattleBase.Gameplay
{
    public class BuildingSiteSelector : IBuildingSiteSelector
    {
        private ISelectable _selectable;

        public bool TrySelect(ISelectable site)
        {
            if (site.TrySelect())
            {
                Unselect();
                _selectable = site;

                return true;
            }

            return false;
        }

        public void Unselect()
        {
            if (_selectable != null)
            {
                _selectable.Unselect();
                _selectable = null;
            }
        }
    }
}