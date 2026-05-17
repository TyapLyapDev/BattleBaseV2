using System;
using System.Collections.Generic;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class Mover : IMover
    {
        private readonly Queue<IWaypoint> _waypoints;

        private IWaypoint _currentWaypoint;

        public event Action<IWaypoint> WaypointChanged;
        public event Action Moved;
        public event Action Stoped;

        public Mover(IMoveConfig moveConfig)
        {
            Config = moveConfig ?? throw new ArgumentNullException(nameof(moveConfig));

            _waypoints = new Queue<IWaypoint>();
        }

        public IMoveConfig Config { get; }

        public bool CanMove => _currentWaypoint != null;

        public void Enable()
        {
            Stop();
        }

        public void Disable()
        {
            _waypoints.Clear();
            _currentWaypoint = null;
        }

        public void AddWaypoints(IEnumerable<IWaypoint> waypoints)
        {
            if (waypoints == null)
                throw new ArgumentNullException(nameof(waypoints));

            foreach (IWaypoint waypoint in waypoints)
                _waypoints.Enqueue(waypoint);
        }

        public void EstablishNextPoint()
        {
            if (_waypoints.Count == 0)
            {
                _currentWaypoint = null;

                return;
            }

            _currentWaypoint = _waypoints.Dequeue();

            WaypointChanged?.Invoke(_currentWaypoint);
        }

        public void Move()
        {
            Moved?.Invoke();
        }

        public void Stop()
        {
            Stoped?.Invoke();
        }
    }
}
