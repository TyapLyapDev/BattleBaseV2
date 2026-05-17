using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    public class ColorSet : MonoBehaviour
    {
        [SerializeField] private ColorSetConfig _config;
        [SerializeField] private ColorBox _prefab;
        [SerializeField] private Transform _context;

        private readonly List<ColorBox> _boxes = new();

        public event Action<int> Clicked;

        public Color CurrentColor { get; private set; }

        public int CurrentColorIndex { get; private set; }

        private void OnEnable()
        {
            foreach (ColorBox box in _boxes)
                box.Clicked += OnColorBoxClick;
        }

        private void OnDisable() =>
            Unsubscribe();

        public void Init(int indexColor)
        {
            if (_config == null)
                throw new ArgumentNullException(nameof(_config), $"{nameof(ColorSet)}: {nameof(_config)} is not assigned in the inspector.");

            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab), $"{nameof(ColorSet)}: {nameof(_prefab)} is not assigned.");

            if (_context == null)
                throw new ArgumentNullException(nameof(_context), $"{nameof(ColorSet)}: {nameof(_context)} is not assigned.");

            if (_config.Colors.Count == 0)
                throw new InvalidOperationException("Config is empty");

            if (indexColor < 0 || indexColor >= _config.Colors.Count)
                throw new ArgumentOutOfRangeException(nameof(indexColor), $"Index {indexColor} is out of range for colors list (size {_config.Colors.Count})");

            CurrentColor = _config.Colors[indexColor];
            CurrentColorIndex = indexColor;
            ClearContext();

            List<Color> colors = new(_config.Colors);

            for (int i = 0; i < colors.Count; i++)
            {
                ColorBox box = Instantiate(_prefab, _context);
                box.Init(colors[i], i);
                box.Deselect();
                _boxes.Add(box);
            }

            _boxes[indexColor].Select();
        }

        public void EnableInteractableAll()
        {
            foreach (ColorBox box in _boxes)
                box.EnableInteractable();
        }

        public void DisableInteractable(int index) =>
            _boxes[index].DisableInteractable();

        private void Unsubscribe()
        {
            foreach (ColorBox box in _boxes)
            {
                if (box != null)
                    box.Clicked -= OnColorBoxClick;
            }
        }

        private void ClearContext()
        {
            Unsubscribe();

            foreach (Transform child in _context)
                Destroy(child.gameObject);

            _boxes.Clear();
        }

        private void OnColorBoxClick(ColorBox colorBox)
        {
            foreach (ColorBox box in _boxes)
                box.Deselect();

            colorBox.Select();
            CurrentColor = colorBox.Color;
            CurrentColorIndex = colorBox.Index;

            Clicked?.Invoke(CurrentColorIndex);
        }
    }
}