using System;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public interface IBuildingSite : IEntity
    {
        public event Action StateChanged;        

        public BuildingSiteState State { get; }

        public bool TrySelect();

        public void Unselect();

        public void SetInactiveState();
    }
}