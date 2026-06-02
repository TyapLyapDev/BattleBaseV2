using System;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    public class MiniMapClickHandler : MonoBehaviour, IInjectable, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private MiniMapArea _miniMapArea;

        private RectTransform _rectTransform;

        private ICameraHandle _cameraHandle;
        private ICameraArea _area;
        private IFrustumProjectionService _frustumService;
        private IInertiaSnapbackApplier _inertiaApplier;
        private ICameraDragger _cameraDragger;
        private ICameraSnapBack _snapBack;

        private bool _isDraggingMiniMap;

        [Inject]
        public void Construct(
            ICameraHandle cameraHandle,
            ICameraArea area,
            IFrustumProjectionService frustumService,
            IInertiaSnapbackApplier inertiaApplier,
            ICameraSnapBack snapBack,
            ICameraDragger cameraDragger)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _area = area ?? throw new ArgumentNullException(nameof(area));
            _frustumService = frustumService ?? throw new ArgumentNullException(nameof(frustumService));
            _inertiaApplier = inertiaApplier ?? throw new ArgumentNullException(nameof(inertiaApplier));
            _cameraDragger = cameraDragger ?? throw new ArgumentNullException(nameof(cameraDragger));
            _snapBack = snapBack ?? throw new ArgumentNullException(nameof(snapBack));
        }

        private void Awake()
        {
            if (_miniMapArea == null)
                throw new NullReferenceException(nameof(_miniMapArea));

            _rectTransform = transform as RectTransform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _cameraDragger.Disable();
            _isDraggingMiniMap = true;
            _inertiaApplier.ResetInertia();
            MoveCameraToPointer(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDraggingMiniMap == false)
                return;

            MoveCameraToPointer(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDraggingMiniMap == false)
                return;

            _isDraggingMiniMap = false;
            _cameraDragger.Enable();
        }

        private void MoveCameraToPointer(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint) == false)
            {
                return;
            }

            Rect rect = _rectTransform.rect;
            float normX = Mathf.Clamp01((localPoint.x - rect.xMin) / rect.width);
            float normY = Mathf.Clamp01((localPoint.y - rect.yMin) / rect.height);

            Bounds bounds = _area.AreaBounds;
            float groundY = _area.GroundPlaneY;
            Vector3 targetWorldPoint = ComputeWorldPoint(bounds, groundY, normX, normY);

            Vector3 delta = targetWorldPoint - _frustumService.Projection.Center;
            
            _cameraHandle.SetCameraRigPosition(_cameraHandle.CameraRig.transform.position + delta);

            _frustumService.Refresh();
            _snapBack.ClampByOvershoot();
        }

        private Vector3 ComputeWorldPoint(Bounds bounds, float groundY, float normX, float normY)
        {
            if (_miniMapArea.Orientation == ScreenOrientationType.Portrait)
            {
                float worldX = bounds.min.x + normX * bounds.size.x;
                float worldZ = bounds.min.z + normY * bounds.size.z;

                return new Vector3(worldX, groundY, worldZ);
            }
            else
            {
                float worldZ = bounds.min.z + normX * bounds.size.z;
                float worldX = bounds.min.x + (1f - normY) * bounds.size.x;

                return new Vector3(worldX, groundY, worldZ);
            }
        }
    }
}