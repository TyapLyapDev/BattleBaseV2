using System;
using System.Collections.Generic;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation.InputReader;
using BattleBase.Gameplay.Map;
using BattleBase.SaveService;
using UnityEngine;
using VContainer;

namespace BattleBase.Mediators
{
    public class MapTerritoryMediator : MediatorBase, ISaveable, IInjectable
    {
        [SerializeField] private List<Territory> _territories;
        [SerializeField] private List<TerritoryConfig> _territoryConfigs;
        [SerializeField] private TerritoryStatusIndicator _territoryStatusIndicatorPrefab;

        private ITerritorySaver _saver;
        private IClickDetector _clickDetector;
        private ITerritorySelector _territorySelector;

        public event Action Changed;

        public IReadOnlyList<Territory> Territories => _territories;

        [Inject]
        public void Construct(
            ITerritorySaver saver,
            IClickDetector clickDetector,
            ITerritorySelector territorySelector)
        {
            _saver = saver ?? throw new ArgumentNullException(nameof(saver));
            _clickDetector = clickDetector ?? throw new ArgumentNullException(nameof(clickDetector));
            _territorySelector = territorySelector ?? throw new ArgumentNullException(nameof(territorySelector));
        }

        private void OnEnable() =>
            _clickDetector.Clicked += OnClick;

        private void OnDisable() =>
            _clickDetector.Clicked -= OnClick;

        public override void Init()
        {
            if (_territories == null)
                throw new NullReferenceException(nameof(_territories));
           
            if (_territoryConfigs == null)
                throw new NullReferenceException(nameof(_territoryConfigs));

            if (_territories.Count != _territoryConfigs.Count)
                throw new InvalidOperationException("Discrepancy between the number of territories and the number of configs was found");

            for (int i = 0; i < _territories.Count; i++)
            {
                Territory territory = _territories[i];
                territory.SetConfig(_territoryConfigs[i]);
                TerritoryStatusIndicator indicator = Instantiate(_territoryStatusIndicatorPrefab);
                indicator.SetTerritory(territory);
            }
        }

        public void Load()
        {
            HashSet<int> conqueredSet = new(_saver.TerritoryData.ConqueredTerritories);

            for (int i = 0; i < _territories.Count; i++)
            {
                TerritoryOwnerType owner = conqueredSet.Contains(i)
                    ? TerritoryOwnerType.Player
                    : TerritoryOwnerType.Enemy;

                _territories[i].SetOwner(owner);
            }

            foreach (Territory territory in _territories)
            {
                if (territory.Owner != TerritoryOwnerType.Player)
                    continue;

                foreach (Territory adjacent in territory.Adjacents)
                {
                    if (adjacent != null && adjacent.Owner != TerritoryOwnerType.Player)
                        adjacent.SetOwner(TerritoryOwnerType.Contested);
                }
            }

            Changed?.Invoke();
        }

        public void Save()
        {
            List<int> conqueredTerritories = new();

            for (int i = 0; i < _territories.Count; i++)
            {
                if (_territories[i].Owner == TerritoryOwnerType.Player)
                    conqueredTerritories.Add(i);
            }

            _saver.SetTerritoryData(new TerritoryData(conqueredTerritories));
        }

        private void OnClick(Collider collider)
        {
            if (collider.TryGetComponent(out Territory territory))
                _territorySelector.Select(territory);
            else
                _territorySelector.Unselect();
        }
    }
}