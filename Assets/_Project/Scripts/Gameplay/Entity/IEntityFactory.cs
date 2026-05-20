using System.Collections.Generic;
using BattleBase.UI;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public interface IEntityFactory
    {
        public void SetBarracksInfos(IReadOnlyList<IProductionItemInfo> barracksItemInfos);

        public void SetMachineFactoryInfos(IReadOnlyList<IProductionItemInfo> machineFactoryItemInfos);

        public T Create<T>(T prefab, Transform target) where T : Entity;
    }
}