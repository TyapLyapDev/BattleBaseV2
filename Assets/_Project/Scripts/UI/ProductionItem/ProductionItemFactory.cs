using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace BattleBase.UI
{
    public class ProductionItemFactory : IProductionItemFactory
    {
        private readonly ProductionItem _itemPrefab;
        private readonly IObjectResolver _resolver;

        public ProductionItemFactory(ProductionItem itemPrefab, IObjectResolver resolver)
        {
            _itemPrefab = itemPrefab != null ? itemPrefab : throw new ArgumentNullException(nameof(itemPrefab));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public List<IProductionItem> Create(IReadOnlyList<IProductionItemInfo> infos)
        {
            if (infos == null)
                return new();

            List<IProductionItem> items = new();

            foreach (IProductionItemInfo info in infos)
            {
                IProductionItem item = _resolver.Instantiate(_itemPrefab);
                item.SetInfo(info);
                items.Add(item);
            }

            return items;
        }
    }
}