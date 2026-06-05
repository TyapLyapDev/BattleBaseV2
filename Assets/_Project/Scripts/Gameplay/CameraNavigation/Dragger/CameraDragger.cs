using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraDragger : ICameraDragger
    {
        private readonly IDragApplier _dragApplier;
        private readonly IInertiaSnapbackApplier _inertiaApplier;
        private readonly ICameraSnapBack _snapBack;

        private bool _enabled = true;

        public CameraDragger(
            IDragApplier dragApplier,
            IInertiaSnapbackApplier inertiaSnapbackApplier,
            ICameraSnapBack snapBack)
        {
            _dragApplier = dragApplier ?? throw new ArgumentNullException(nameof(dragApplier));
            _inertiaApplier = inertiaSnapbackApplier ?? throw new ArgumentNullException(nameof(inertiaSnapbackApplier));
            _snapBack = snapBack ?? throw new ArgumentNullException(nameof(snapBack));
        }

        public void Enable() =>
            _enabled = true;

        public void Disable() =>
            _enabled = false;

        public void Update(float deltaTime, Vector3? worldDragDelta)
        {
            if (_enabled == false)
                return;

            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));

            if (worldDragDelta.HasValue)
            {
                _inertiaApplier.UpdateInertia(worldDragDelta.Value, deltaTime);
                _dragApplier.Apply(worldDragDelta.Value);
            }
            else
            {
                _inertiaApplier.Apply(deltaTime);
            }

            _snapBack.ClampByOvershoot();
        }

        public void RestorePosition(Vector3 currentPosition, Vector3 positionToRestore)
        {
            _dragApplier.RestorePosition(currentPosition, positionToRestore);
            _snapBack.ClampByOvershoot();
        }
    }
}