namespace BattleBase.Gameplay
{
    public class BuildingSiteSelector : IBuildingSiteSelector
    {
        private IBuildingSite _selected;

        public bool TrySelect(IBuildingSite site)
        {
            if (site.TrySelect())
            {
                Unselect();
                _selected = site;

                return true;
            }

            return false;
        }

        public void Unselect()
        {
            if (_selected != null)
            {
                _selected.Unselect();
                _selected = null;
            }
        }
    }
}