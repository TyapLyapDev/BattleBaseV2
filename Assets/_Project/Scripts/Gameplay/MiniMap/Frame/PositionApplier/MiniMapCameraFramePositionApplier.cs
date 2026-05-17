using System;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    [RequireComponent(typeof(MiniMapCameraFrame))]
    public class MiniMapCameraFramePositionApplier : MonoBehaviour, IInjectable
    {
        [SerializeField] private MiniMapArea _miniMapArea;

        private MiniMapCameraFrame _frame;
        private ICameraAreaService _cameraAreaService;
        private IFrustumProjectionService _frustumProjectionService;
        private ICameraTracker _cameraTracker;
        private IFramePositionCalculator _calculator;

        [Inject]
        public void Construct(
            ICameraTracker cameraTracker,
            ICameraAreaService cameraAreaService,
            IFrustumProjectionService frustumProjectionService)
        {
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));
            _cameraAreaService = cameraAreaService ?? throw new ArgumentNullException(nameof(cameraAreaService));
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));

            _frame = GetComponent<MiniMapCameraFrame>();

            _calculator = _miniMapArea.Orientation == ScreenOrientationType.Portrait
                ? new VerticalFramePositionCalculator()
                : new HorizontalFramePositionCalculator();
        }

        private void OnEnable()
        {
            _miniMapArea.SizeChanged += OnAreaChanged;
            _cameraTracker.PositionChanged += UpdatePosition;
            _cameraAreaService.Changed += UpdatePosition;
            OnAreaChanged();
            UpdatePosition();
        }

        private void OnDisable()
        {
            _miniMapArea.SizeChanged -= OnAreaChanged;
            _cameraTracker.PositionChanged -= UpdatePosition;
            _cameraAreaService.Changed -= UpdatePosition;
        }

        private void OnAreaChanged() =>
            UpdatePosition();

        private void UpdatePosition()
        {
            FramePositionInput input = new()
            {
                WorldCenter = _frustumProjectionService.ProjectedCenter,
                AreaBounds = _cameraAreaService.AreaBounds,
                MiniMapRect = _miniMapArea.Rect
            };

            Vector2 position = _calculator.Calculate(input);
            _frame.SetAnchoredPosition(position);
        }
    }
}