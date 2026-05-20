using System;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public abstract class Building : Entity, ISelectable
    {
        [SerializeField] private Color _color;

        public IBuildingSite Site { get; private set; }

        public Transform UnitSpawnPoint { get; private set; }

        private void Awake() =>
            SetColor(_color);

        public void SetBuildingSite(IBuildingSite site)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
            UnitSpawnPoint = site.UnitSpawnPoint;
        }

        public void SetUnitSpawnPoint(Transform point) =>
            UnitSpawnPoint = point != null ? point : throw new ArgumentNullException(nameof(point));

        public bool TrySelect()
        {
            Debug.Log($"Выбрано здание {name}");

            return true;
        }

        public void Unselect()
        {
            Debug.Log($"Снят выбор со здания {name}");
        }
    }
}