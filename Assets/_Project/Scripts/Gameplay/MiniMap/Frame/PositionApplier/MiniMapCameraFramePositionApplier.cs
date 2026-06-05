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
        private ICameraArea _cameraArea;
        private IFrustumProjectionService _frustumProjectionService;
        private ICameraHandle _cameraTracker;
        private IFramePositionCalculator _calculator;

        [Inject]
        public void Construct(
            ICameraHandle cameraTracker,
            ICameraArea cameraArea,
            IFrustumProjectionService frustumProjectionService)
        {
            _cameraTracker = cameraTracker ?? throw new ArgumentNullException(nameof(cameraTracker));
            _cameraArea = cameraArea ?? throw new ArgumentNullException(nameof(cameraArea));
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
            _cameraArea.Changed += UpdatePosition;
            OnAreaChanged();
            UpdatePosition();
        }

        private void OnDisable()
        {
            _miniMapArea.SizeChanged -= OnAreaChanged;
            _cameraTracker.PositionChanged -= UpdatePosition;
            _cameraArea.Changed -= UpdatePosition;
        }

        private void OnAreaChanged() =>
            UpdatePosition();

        private void UpdatePosition()
        {
            FramePositionInput input = new()
            {
                WorldCenter = _frustumProjectionService.Projection.Center,
                AreaBounds = _cameraArea.AreaBounds,
                MiniMapRect = _miniMapArea.Rect,
            };

            Vector2 position = _calculator.Calculate(input);
            _frame.SetAnchoredPosition(position);
        }
    }
}