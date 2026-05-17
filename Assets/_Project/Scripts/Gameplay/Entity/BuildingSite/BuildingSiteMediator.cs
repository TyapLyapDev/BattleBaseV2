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
        [SerializeField] private CommandShowHidePopUps _commandShowProductionPanel;
        [SerializeField] private CommandShowHidePopUps _commandHideProductionPanel;

        private List<IProductionItem> _items = new();

        private IBuildingSite _currentSite;
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
            _currentSite = null;

            if (entity is IBuildingSite buildingSite)
            {
                _currentSite = buildingSite;
                _selector.TrySelect(buildingSite);
            }

            _productionPanel.ClearContext();
            _items = _productionItemFactory.Create(entity.ProductionItemInfos);

            if (_items.Count == 0)
                _commandHideProductionPanel.Execute();
            else
                _commandShowProductionPanel.Execute();

            foreach (IProductionItem item in _items)
            {
                _productionPanel.AddItem(item);
                item.ItemClicked += OnItemClick;
            }
        }

        private void HandleUnselectEntity()
        {
            _selector.Unselect();
            _commandHideProductionPanel.Execute();

            foreach (IProductionItem item in _items)
                item.ItemClicked -= OnItemClick;

            _items.Clear();
        }

        private void OnItemClick(IProductionItem item)
        {
            Transform target = _currentSite.Transform;
            IEntity entity = _entityFactory.Create(item.Info.Prefab, target);
            entity.SetPlayerMarker();

            if (entity is Building building)
                building.SetBuildingSite(_currentSite);

            _currentSite.SetInactiveState();
            HandleUnselectEntity();
        }
    }
}