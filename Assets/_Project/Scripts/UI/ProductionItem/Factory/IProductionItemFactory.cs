using System.Collections.Generic;

namespace BattleBase.UI
{
    public interface IProductionItemFactory
    {
        public List<IProductionItem> Create(IReadOnlyList<IProductionItemInfo> infos);
    }
}