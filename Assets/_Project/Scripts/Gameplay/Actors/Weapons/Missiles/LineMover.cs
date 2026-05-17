using BattleBase.Utils;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons.Missiles
{
    public class LineMover : MonoBehaviour, IMissileMover
    {
        [SerializeField][Range(0, 1f)] private float _finishDistance = 0.05f;
        [SerializeField][Min(10f)] private float _speed = 50f;

        private Transform _transform;
        private Vector3 _pointPosition;

        public Vector3 CurrentPosition => _transform.position;

        public bool IsFinished => VectorTool.IsWithinDistance(_transform.position, _pointPosition, _finishDistance);

        private void Awake()
        {
            _transform = transform;
        }

        public void SetStartPosition(Vector3 startPosition) => _transform.position = startPosition;

        public void SetPointPosition(Vector3 point) => _pointPosition = point;

        public void Move(float delta)
        {
            if (delta <= 0)
                throw new ArgumentOutOfRangeException(nameof(delta));

            _transform.position = Vector3.MoveTowards(_transform.position, _pointPosition, delta * _speed);
        }
    }
}