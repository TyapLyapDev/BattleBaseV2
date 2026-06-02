using System;
using System.Collections.Generic;
using BattleBase.Commands;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation.InputReader;
using BattleBase.UI;
using BattleBase.UI.PopUps;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay
{
    public class BuildingSiteMediator : MonoBehaviour, IInjectable
    {
        [SerializeField] private ProductionPanel _productionPanel;

        private List<IProductionItem> _items = new();

        private IEntity _selectedEntity;
        private IClickDetector _clickDetector;
        private IBuildingSiteSelector _selector;
        private IProductionItemFactory _productionItemFactory;
        private IEntityFactory _entityFactory;

        [Inject]
        public void Construct(
            IClickDetector clickDetector,
            IBuildingSiteSelector selector,
            IProductionItemFactory productionItemFactory,
            IEntityFactory entityFactory)
        {
            _clickDetector = clickDetector ?? throw new ArgumentNullException(nameof(clickDetector));
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _productionItemFactory = productionItemFactory ?? throw new ArgumentNullException(nameof(productionItemFactory));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        }

        private void OnEnable() =>
            _clickDetector.Clicked += OnClickDetected;

        private void OnDisable() =>
            _clickDetector.Clicked -= OnClickDetected;

        private void OnClickDetected(Collider collider)
        {
            if (collider == null)
                return;

            if (collider.TryGetComponent(out IEntity entity))
            {
                if (entity.IsPlayer)
                {
                    HandleSelectEntity(entity);

                    return;
                }
            }

            HandleUnselectEntity();
        }

        private void HandleSelectEntity(IEntity entity)
        {
            _selectedEntity = null;

            if (entity is ISelectable selectable)
            {
                _selectedEntity = entity;
                _selector.TrySelect(selectable);
            }

            _productionPanel.ClearContext();
            _items = _productionItemFactory.Create(entity.ProductionItemInfos);

            if (_items.Count == 0)
                _productionPanel.Hide();
            else
                _productionPanel.Show();

            foreach (IProductionItem item in _items)
            {
                _productionPanel.AddItem(item);
                item.ItemClicked += OnItemClick;
            }
        }

        private void HandleUnselectEntity()
        {
            _selector.Unselect();
            _productionPanel.Hide();

            foreach (IProductionItem item in _items)
                item.ItemClicked -= OnItemClick;

            _items.Clear();
        }

        private void OnItemClick(IProductionItem item)
        {
            if (_selectedEntity == null)
                return;

            Entity prefab = item.Info.Prefab;

            if (prefab is Building buildingPrefab)
            {
                if (_selectedEntity is IBuildingSite buildingSite)
                {
                    Transform target = _selectedEntity.Transform;
                    Building newBuilding = _entityFactory.Create(buildingPrefab, target);
                    newBuilding.SetPlayerMarker();
                    newBuilding.SetBuildingSite(buildingSite);
                    buildingSite.SetInactiveState();
                    HandleSelectEntity(newBuilding);
                }
            }
            else if (prefab is Unit unitPrefab)
            {
                if (_selectedEntity is Building building)
                {
                    Transform target = building.UnitSpawnPoint;
                    Unit unit = _entityFactory.Create(unitPrefab, target);
                    unit.SetPlayerMarker();
                }
            }
            else
            {
                HandleUnselectEntity();
            }
        }
    }
}