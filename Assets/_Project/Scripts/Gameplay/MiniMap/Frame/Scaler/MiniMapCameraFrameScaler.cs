using System;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    [RequireComponent(typeof(MiniMapCameraFrame))]
    public class MiniMapCameraFrameScaler : MonoBehaviour, IInjectable
    {
        [SerializeField] private MiniMapArea _area;

        private MiniMapCameraFrame _frame;
        private ICameraAreaService _areaService;
        private ICameraZoom _cameraZoom;
        private IFrustumProjectionService _frustumProjectionService;
        private IFrameSizeCalculator _calculator;

        [Inject]
        public void Construct(
            ICameraAreaService areaService,
            ICameraZoom cameraZoom,
            IFrustumProjectionService projectionService)
        {
            _areaService = areaService ?? throw new ArgumentNullException(nameof(areaService));
            _cameraZoom = cameraZoom ?? throw new ArgumentNullException(nameof(cameraZoom));
            _frustumProjectionService = projectionService ?? throw new ArgumentNullException(nameof(projectionService));
            
            _frame = GetComponent<MiniMapCameraFrame>();

            _calculator = _area.Orientation == ScreenOrientationType.Portrait
                    ? new VerticalFrameSizeCalculator()
                    : new HorizontalFrameSizeCalculator();
        }

        private void OnEnable()
        {
            _cameraZoom.Changed += Refresh;
            _area.SizeChanged += Refresh;
            _frustumProjectionService.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            _cameraZoom.Changed -= Refresh;
            _area.SizeChanged -= Refresh;
            _frustumProjectionService.Changed -= Refresh;
        }

        private void Refresh()
        {
            Bounds bounds = _areaService.AreaBounds;

            FrameSizeInput input = new()
            {
                FrustumSize = new Vector2(_frustumProjectionService.CachedWidth, _frustumProjectionService.CachedHeight),
                WorldAreaSize = new Vector2(bounds.size.x, bounds.size.z),
                MiniMapAreaSize = new Vector2(_area.Rect.width, _area.Rect.height),
            };

            Vector2 size = _calculator.Calculate(input);

            _frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            _frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}