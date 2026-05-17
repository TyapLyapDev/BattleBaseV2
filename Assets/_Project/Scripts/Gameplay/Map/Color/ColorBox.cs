using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleBase.Gameplay.Map
{
    public class ColorBox : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _frame;
        [SerializeField] private Button _button;

        public event Action<ColorBox> Clicked;

        public Color Color {  get; private set; }

        public int Index { get; private set; } = -1;

        private void OnEnable() =>
            _button.onClick.AddListener(OnClick);

        private void OnDisable() =>
            _button.onClick.RemoveListener(OnClick);

        public void Init(Color color, int index)
        {
            Color = color;
            Index = index;
            _image.color = color;
        }

        public void Select() =>
            _frame.gameObject.SetActive(true);

        public void Deselect() =>
            _frame.gameObject.SetActive(false);

        public void EnableInteractable() =>
            _button.interactable = true;

        public void DisableInteractable() =>
            _button.interactable = false;

        private void OnClick() =>
            Clicked?.Invoke(this);
    }
}