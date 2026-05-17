using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    public class Territory : MonoBehaviour
    {
        [SerializeField] private List<Territory> _adjacents;
        
        private TerritoryConfig _config;

        public event Action ColorChanged;
        public event Action OwnerChanged;

        public Transform Target => transform;

        public Color? Color {  get; private set; }

        public TerritoryOwnerType Owner {  get; private set; }

        public ITerritoryInfo Info => _config;

        public IReadOnlyList<Territory> Adjacents => _adjacents;

        public void SetConfig(TerritoryConfig config) =>
            _config = config != null ? config : throw new ArgumentNullException(nameof(config));

        public void SetColor(Color color)
        {
            Color = color;
            ColorChanged?.Invoke();
        }

        public void SetOwner(TerritoryOwnerType owner)
        {
            Owner = owner;
            OwnerChanged?.Invoke();
        }

        // todo : вынести в редакторский скрипт
#if UNITY_EDITOR
        public void AddAdjacent(Territory territory)
        {
            if (territory == null || territory == this) 
                return;

            if (_adjacents.Contains(territory) == false)
            {
                _adjacents.Add(territory);
                territory.AddAdjacentInternal(this);
            }
        }

        public void RemoveAdjacent(Territory territory)
        {
            if (_adjacents.Remove(territory))
                territory.RemoveAdjacentInternal(this);
        }

        private void AddAdjacentInternal(Territory territory)
        {
            if (_adjacents.Contains(territory) == false)
                _adjacents.Add(territory);
        }

        private void RemoveAdjacentInternal(Territory territory) =>
            _adjacents.Remove(territory);
#endif
    }
}