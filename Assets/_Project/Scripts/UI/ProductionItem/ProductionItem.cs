using System;
using BattleBase.Commands;
using BattleBase.DI;
using BattleBase.UI.Buttons;
using BattleBase.UI.PopUps;
using BattleBase.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace BattleBase.UI
{
    public class ProductionItem : MonoBehaviour, IProductionItem, IInjectable
    {
        [SerializeField] private Image _icon;
        [SerializeField] private ButtonClickHandler _itemButton;
        [SerializeField] private ButtonClickHandler _moreInfoButton;
        [SerializeField] private TMP_Text _price;

        private ItemInfoPopUp _popUp;

        public event Action<ProductionItem> ItemClicked;

        public IProductionItemInfo Info { get; private set; }

        [Inject]
        public void Construct(ItemInfoPopUp popUp, [Key(Constants.KeyCommandShowItemInfoPopUp)] CommandBase commandShowItemInfoPopUp)
        {
            _popUp = popUp != null ? popUp : throw new ArgumentNullException(nameof(popUp));
            _moreInfoButton.AddCommand(commandShowItemInfoPopUp);
        }

        private void OnEnable()
        {
            _itemButton.Clicked += OnItemButton;
            _moreInfoButton.Clicked += OnMoreInfoClicked;
        }

        private void OnDisable()
        {
            _itemButton.Clicked -= OnItemButton;
            _moreInfoButton.Clicked -= OnMoreInfoClicked;
        }
            
        public void SetParent(Transform parent) =>
            transform.SetParent(parent, false);

        public void ResetParent() =>
            transform.SetParent(null, false);

        public void SetInfo(IProductionItemInfo info)
        {
            Info = info;

            _icon.sprite = Info.Sprite;
            _price.text = Info.Price.ToString();
        }

        private void OnItemButton(ButtonClickHandler handler) =>
            ItemClicked?.Invoke(this);

        private void OnMoreInfoClicked(ButtonClickHandler handler) =>
            _popUp.SetInfo(Info);
    }
}