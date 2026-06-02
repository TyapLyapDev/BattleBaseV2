using System;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    [RequireComponent(typeof(MiniMapArea))]
    public class MiniMapAreaScaler : MonoBehaviour, IInjectable
    {
        private MiniMapArea _miniMapArea;
        private ICameraArea _cameraArea;
        private IAreaSizeCalculator _calculator;

        [Inject]
        public void Construct(ICameraArea area)
        {
            _cameraArea = area ?? throw new ArgumentNullException(nameof(area));
            _miniMapArea = GetComponent<MiniMapArea>();

            _calculator = _miniMapArea.Orientation == ScreenOrientationType.Portrait
                ? new VerticalAreaSizeCalculator()
                : new HorizontalAreaSizeCalculator();
        }

        private void OnEnable()
        {
            _cameraArea.Changed += UpdateAreaSize;
            UpdateAreaSize();
        }

        private void OnDisable() =>
            _cameraArea.Changed -= UpdateAreaSize;

        private void UpdateAreaSize()
        {
            Bounds bounds = _cameraArea.AreaBounds;
            Vector2 worldSize = new(bounds.size.x, bounds.size.z);
            Vector2 currentSize = new(_miniMapArea.Rect.width, _miniMapArea.Rect.height);

            AreaSizeInput input = new()
            {
                WorldSize = worldSize,
                CurrentMiniMapSize = currentSize
            };

            Vector2 newSize = _calculator.CalculateNewSize(input);

            _miniMapArea.SetSizeWithCurrentAnchors(newSize);
        }
    }
}