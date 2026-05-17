using System;
using System.Collections.Generic;
using BattleBase.UI;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [SerializeField] private bool _isPlayer;

        private List<IProductionItemInfo> _productionItemInfos;

        public event Action<IEntity> Deactivated;
        public event Action<IEntity> ColorChanged;

        public Transform Transform => transform;

        public IReadOnlyList<IProductionItemInfo> ProductionItemInfos => _productionItemInfos;

        public Color Color { get; private set; }

        public bool IsPlayer => _isPlayer;

        public void SetItemInfos(IReadOnlyList<IProductionItemInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException(nameof(infos));

            _productionItemInfos = new(infos);
        }

        public void SetPlayerMarker() =>
            _isPlayer = true;

        public void SetColor(Color color)
        {
            Color = color;
            ColorChanged?.Invoke(this);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this);
        }
    }
}