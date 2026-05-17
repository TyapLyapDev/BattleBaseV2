using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class WaypointController : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;

        private List<Waypoint> _waypoints;

        private void Awake()
        {
            _waypoints = new List<Waypoint>();

            foreach (var point in _points)
            {
                Waypoint waypoint = new(point.position);
                _waypoints.Add(waypoint);
            }
        }

        public void SpecifyActorRoute(IActor actor)
        {
            if (actor == null) 
                throw new System.ArgumentNullException(nameof(actor));

            if (actor.TryGetComponent(out IMover mover) == false)
                return;

            mover.AddWaypoints(_waypoints.ToArray());
            mover.EstablishNextPoint();
            mover.Move();
        } 
    }
}