using UnityEngine;

namespace BattleBase.UI.PopUps
{
    public class ProductionPanel : PopUp
    {
        public void ClearContext()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }

        public void AddItem(IProductionItem item) =>
            item.SetParent(transform);
    }
}