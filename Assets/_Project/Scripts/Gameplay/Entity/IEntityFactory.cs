using System.Collections.Generic;
using BattleBase.UI;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public interface IEntityFactory
    {
        public void SetBarracksInfos(IReadOnlyList<IProductionItemInfo> barracksItemInfos);

        public void SetMachineFactoryInfos(IReadOnlyList<IProductionItemInfo> machineFactoryItemInfos);

        public IEntity Create(Entity prefab, Transform target);
    }
}