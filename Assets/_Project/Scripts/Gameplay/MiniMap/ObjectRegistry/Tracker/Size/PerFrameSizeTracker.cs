using System;
using BattleBase.UpdateService;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class PerFrameSizeTracker : IEntitySizeTracker
    {
        private readonly Transform _transform;
        private readonly IUpdater _updater;
        private readonly IEntitySizeCalculator _sizeCalculator;

        public PerFrameSizeTracker(Transform transform, IEntitySizeCalculator sizeCalculator, IUpdater updater)
        {
            _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
            _sizeCalculator = sizeCalculator ?? throw new ArgumentNullException(nameof(sizeCalculator));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));

            WorldSize = sizeCalculator.Calculate(transform);

            _updater.Subscribe(OnUpdate, UpdateType.Update);
        }

        public event Action Changed;

        public Vector2 WorldSize { get; private set; }

        public void Dispose() =>
            _updater.Unsubscribe(OnUpdate, UpdateType.Update);

        private void OnUpdate()
        {
            Vector2 worldSize = _sizeCalculator.Calculate(_transform);

            if (WorldSize != worldSize)
            {
                WorldSize = worldSize;
                Changed?.Invoke();
            }
        }
    }
}