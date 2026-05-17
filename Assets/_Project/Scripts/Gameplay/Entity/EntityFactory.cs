using System;
using System.Collections.Generic;
using BattleBase.Gameplay.MiniMap;
using BattleBase.UI;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public class EntityFactory : IEntityFactory
    {
        private List<IProductionItemInfo> _barracksItemInfos;
        private List<IProductionItemInfo> _machineFactoryItemInfos;
        private readonly IEntityTrackerFactory _trackerFactory;

        public EntityFactory(IEntityTrackerFactory trackerFactory)
        {
            _trackerFactory = trackerFactory ?? throw new ArgumentNullException(nameof(trackerFactory));
        }

        public void SetBarracksInfos(IReadOnlyList<IProductionItemInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException(nameof(_barracksItemInfos));

            _barracksItemInfos = new(infos);
        }

        public void SetMachineFactoryInfos(IReadOnlyList<IProductionItemInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException(nameof(_barracksItemInfos));

            _machineFactoryItemInfos = new(infos);
        }

        public IEntity Create(Entity prefab, Transform target)
        {
            IEntity entity = UnityEngine.Object.Instantiate(prefab, target.position, target.rotation);
            _trackerFactory.CreateTracker(entity, PositionTrackingType.Static);

            if (entity is Barracks)
                entity.SetItemInfos(_barracksItemInfos);
            else if (entity is MachineFactory)
                entity.SetItemInfos(_machineFactoryItemInfos);

            return entity;
        }
    }
}