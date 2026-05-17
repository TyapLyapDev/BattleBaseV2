using System;
using System.Collections.Generic;
using BattleBase.UI;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public interface IEntity
    {
        public event Action<IEntity> Deactivated;
        public event Action<IEntity> ColorChanged;

        public bool IsPlayer { get; }

        public Transform Transform { get; }

        public IReadOnlyList<IProductionItemInfo> ProductionItemInfos { get; }

        public Color Color { get; }

        public void SetItemInfos(IReadOnlyList<IProductionItemInfo> itemConfigs);

        public void SetPlayerMarker();
    }
}