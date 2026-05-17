using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class Waypoint : IWaypoint
    {
        public Waypoint(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; }
    }
}