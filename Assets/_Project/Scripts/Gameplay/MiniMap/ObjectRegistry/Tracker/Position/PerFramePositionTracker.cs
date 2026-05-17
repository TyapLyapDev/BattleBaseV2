using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class PerFramePositionTracker : IEntityPositionTracker
    {
        private readonly Transform _transform;
        private readonly IUpdater _updater;

        public PerFramePositionTracker(Transform transform, IUpdater updater)
        {
            _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));

            WorldPosition = transform.position;
            _updater.Subscribe(OnUpdate, UpdateType.Update);
        }

        public event Action Changed;

        public Vector3 WorldPosition { get; private set; }

        public void Dispose() =>
            _updater.Unsubscribe(OnUpdate, UpdateType.Update);

        private void OnUpdate()
        {
            if (WorldPosition != _transform.position)
            {
                WorldPosition = _transform.position;
                Changed?.Invoke();
            }
        }
    }
}