using System;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public abstract class Building : Entity
    {
        [SerializeField] private Color _color;

        public IBuildingSite Site { get; private set; }

        private void Awake() =>
            SetColor(_color);

        public void SetBuildingSite(IBuildingSite site)
        {
            Site = site ?? throw new ArgumentNullException(nameof(site));
        }
    }
}