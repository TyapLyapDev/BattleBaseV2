namespace BattleBase.Gameplay.Actors.Movement
{
    public class MoverPresenter : IMoverPresenter
    {
        private readonly IMover _model;

        public MoverPresenter(IMover model)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
        }

        public void ReachPoint()
        {
            _model.EstablishNextPoint();

            if (_model.CanMove)
                _model.Move();
        }
    }
}
