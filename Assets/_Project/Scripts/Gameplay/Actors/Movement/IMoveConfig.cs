namespace BattleBase.Gameplay.Actors.Movement
{
    public interface IMoveConfig
    {
        public float Speed { get; }

        public float AngularSpeed { get; }

        public float Acceleration { get; }

        public float StoppingDistance { get; }
    }
}
