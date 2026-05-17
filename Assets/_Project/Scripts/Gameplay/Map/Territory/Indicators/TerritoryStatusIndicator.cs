using System;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    public class TerritoryStatusIndicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite _base;
        [SerializeField] private Sprite _battle;
        [SerializeField] private float _ownerColorBlackoutFactor = 0.2f;

        private Territory _territory;
        private bool _subscribed;

        private void OnEnable() =>
            Subscribe();

        private void OnDisable() =>
            Unsubscribe();

        public void SetTerritory(Territory territory)
        {
            _territory = territory != null ? territory : throw new ArgumentNullException(nameof(territory));

            Transform territoryTransform = territory.transform;
            transform.SetParent(territoryTransform);
            transform.position = territoryTransform.position;

            Subscribe();
        }

        private void Subscribe()
        {
            if(_territory == null)
                return;

            if(_subscribed) 
                return;

            _subscribed = true;

            _territory.OwnerChanged += OnOwnerChanged;
            OnOwnerChanged();

            _territory.ColorChanged += OnColorChanged;
            OnColorChanged();
        }

        private void Unsubscribe()
        {
            if (_territory == null)
                return;

            if (_subscribed == false)
                return;

            _subscribed = false;

            _territory.OwnerChanged -= OnOwnerChanged;
            _territory.ColorChanged -= OnColorChanged;
        }

        private void OnOwnerChanged()
        {
            if (_base == null || _battle == null)
            {
                Debug.LogError($"{nameof(TerritoryStatusIndicator)}: {nameof(_base)} or {nameof(_battle)} sprite is not assigned.", this);

                return;
            }

            Sprite sprite = _territory.Owner switch
            {
                TerritoryOwnerType.Enemy => _base,
                TerritoryOwnerType.Player => _base,
                TerritoryOwnerType.Contested => _battle,
                _ => throw new ArgumentOutOfRangeException(nameof(_territory.Owner), _territory.Owner, $"Type is not registered"),
            };

            _renderer.sprite = sprite;

            OnColorChanged();
        }

        private void OnColorChanged()
        {
            if (_territory.Color.HasValue == false)
                return;

            _renderer.color = _territory.Owner switch
            {
                TerritoryOwnerType.Enemy => ModifyColor(_territory.Color.Value),
                TerritoryOwnerType.Player => ModifyColor(_territory.Color.Value),
                TerritoryOwnerType.Contested => Color.white,
                _ => throw new ArgumentOutOfRangeException(nameof(_territory.Owner), _territory.Owner, $"Type is not registered"),
            };
        }

        private Color ModifyColor(Color color) =>
            Color.Lerp(color, Color.black, _ownerColorBlackoutFactor);
    }
}