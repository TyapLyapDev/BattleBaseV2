using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    [System.Serializable]
    public class MoveConfig : IMoveConfig
    {
        [SerializeField][Min(0.5f)] private float _speed = 3.5f;
        [SerializeField][Min(45f)] private float _angularSpeed = 120f;
        [SerializeField][Min(1f)] private float _acceleration = 10f;
        [SerializeField][Min(0.1f)] private float _stoppingDistance = 3f;

        public float Speed => _speed;

        public float AngularSpeed => _angularSpeed;

        public float Acceleration => _acceleration;

        public float StoppingDistance => _stoppingDistance;
    }
}
