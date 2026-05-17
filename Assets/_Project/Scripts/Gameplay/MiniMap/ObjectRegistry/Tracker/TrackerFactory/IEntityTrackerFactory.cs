namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntityTrackerFactory
    {
        public IEntityTracker CreateTracker(IEntity entity, PositionTrackingType positionTrackingType);
    }
}