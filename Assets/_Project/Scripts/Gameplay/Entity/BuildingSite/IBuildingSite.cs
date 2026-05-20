using System;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public interface IBuildingSite : IEntity, ISelectable
    {
        public event Action StateChanged;

        public Transform UnitSpawnPoint { get; }

        public BuildingSiteState State { get; }

        public void SetInactiveState();
    }
}