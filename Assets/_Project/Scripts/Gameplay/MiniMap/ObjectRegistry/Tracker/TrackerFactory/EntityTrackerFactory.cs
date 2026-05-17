using System;
using BattleBase.UpdateService;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    public class EntityTrackerFactory : IEntityTrackerFactory
    {
        private readonly IEntityTrackersRegistry _entityRegistry;
        private readonly IObjectResolver _resolver;
        private readonly IUpdater _updater;

        public EntityTrackerFactory(IEntityTrackersRegistry entityRegistry, IObjectResolver resolver, IUpdater updater)
        {
            _entityRegistry = entityRegistry ?? throw new ArgumentNullException(nameof(entityRegistry));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _updater = updater ?? throw new ArgumentNullException(nameof(updater));
        }

        public IEntityTracker CreateTracker(IEntity entity, PositionTrackingType positionTrackingType)
        {
            Transform transform = entity.Transform;

            IEntityTracker tracker = new EntityTracker(
                entity,
                CreateSizeTracker(transform),
                CreatePositionTracker(transform, positionTrackingType),
                CreateRotationTracker(transform));

            _entityRegistry.Register(tracker);

            return tracker;
        }

        private IEntitySizeTracker CreateSizeTracker(Transform transform)
        {
            IEntitySizeCalculator sizeCalculator = _resolver.Resolve<IEntitySizeCalculator>();
            IEntitySizeTracker sizeTracker = new StaticSizeTracker(transform, sizeCalculator);

            return sizeTracker;
        }

        private IEntityPositionTracker CreatePositionTracker(Transform transform, PositionTrackingType trackingType)
        {
            return trackingType switch
            {
                PositionTrackingType.Static => new StaticPositionTracker(transform),
                PositionTrackingType.PerFrame => new PerFramePositionTracker(transform, _updater),
                _ => throw new NotImplementedException(),
            };
        }

        private IEntityRotationTracker CreateRotationTracker(Transform transform)
        {
            IEntityRotationTracker rotationTracker = new FixedRotationTracker(transform);

            return rotationTracker;
        }
    }
}