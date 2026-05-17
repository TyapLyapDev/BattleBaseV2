using System;
using System.Collections.Generic;
using BattleBase.Gameplay;
using BattleBase.Gameplay.MiniMap;
using BattleBase.UI;
using UnityEngine;
using VContainer;

namespace BattleBase.EntryPoints
{
    public class GameEntryPoint : EntryPointBase
    {
        [SerializeField] private List<ProductionItemConfig> _buildingSiteItemInfos;
        [SerializeField] private List<ProductionItemConfig> _barracksItemInfos;
        [SerializeField] private List<ProductionItemConfig> _machineFactoryItemInfos;
        [SerializeField] private Transform _environment;

        private IEntityTrackerFactory _trackerFactory;
        private IEntityFactory _entityFactory;

        [Inject]
        public void Construct(
            IEntityTrackerFactory trackerFactory,
            IEntityFactory entityFactory)
        {
            _trackerFactory = trackerFactory ?? throw new ArgumentNullException(nameof(trackerFactory));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        }

        protected override void Start()
        {
            base.Start();

            foreach (Entity entity in _environment.GetComponentsInChildren<Entity>(false))
            {
                if (entity is RoadLane or Stronghold or IBuildingSite)
                {
                    _trackerFactory.CreateTracker(entity, PositionTrackingType.Static);

                    if (entity is IBuildingSite site)
                        site.SetItemInfos(_buildingSiteItemInfos);
                }
            }

            _entityFactory.SetBarracksInfos(_barracksItemInfos);
            _entityFactory.SetMachineFactoryInfos(_machineFactoryItemInfos);
        }
    }
}