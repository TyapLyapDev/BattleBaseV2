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
        private ICameraArea _cameraArea;
        private ICameraZoom _cameraZoom;
        private IFrustumProjectionService _frustumProjectionService;
        private IFrameSizeCalculator _calculator;

        [Inject]
        public void Construct(
            ICameraArea area,
            ICameraZoom cameraZoom,
            IFrustumProjectionService projectionService)
        {
            _cameraArea = area ?? throw new ArgumentNullException(nameof(area));
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
            Bounds bounds = _cameraArea.AreaBounds;

            GroundProjection frustum = _frustumProjectionService.GetProjection(
                FrustumSizeType.MinimumWidthAndHeight, 
                FrustumShape.Rectangle);

            FrameSizeInput input = new()
            {
                FrustumSize = new Vector2(frustum.Width, frustum.Height),
                WorldAreaSize = new Vector2(bounds.size.x, bounds.size.z),
                MiniMapAreaSize = new Vector2(_area.Rect.width, _area.Rect.height),
            };

            Vector2 size = _calculator.Calculate(input);

            _frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            _frame.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}