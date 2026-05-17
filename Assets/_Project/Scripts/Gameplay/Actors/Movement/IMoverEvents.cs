using System;

namespace BattleBase.Gameplay.Actors.Movement
{
    public interface IMoverEvents
    {
        public event Action<IWaypoint> WaypointChanged;

        public event Action Moved;

        public event Action Stoped;
    }
}
