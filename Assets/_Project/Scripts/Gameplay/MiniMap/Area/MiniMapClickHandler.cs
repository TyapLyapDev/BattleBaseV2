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

        private CameraRig _cameraRig;
        private ICameraAreaService _areaService;
        private IFrustumProjectionService _frustumService;
        private IPositionRestrictor _positionRestrictor;
        private IInertiaSnapbackApplier _inertiaApplier;
        private ICameraDragger _cameraDragger;

        private bool _isDraggingMiniMap;

        [Inject]
        public void Construct(
            CameraRig cameraRig,
            ICameraAreaService areaService,
            IFrustumProjectionService frustumService,
            IPositionRestrictor positionRestrictor,
            IInertiaSnapbackApplier inertiaApplier,
            ICameraDragger cameraDragger)
        {
            _cameraRig = cameraRig != null ? cameraRig : throw new ArgumentNullException(nameof(cameraRig));
            _areaService = areaService ?? throw new ArgumentNullException(nameof(areaService));
            _frustumService = frustumService ?? throw new ArgumentNullException(nameof(frustumService));
            _positionRestrictor = positionRestrictor ?? throw new ArgumentNullException(nameof(positionRestrictor));
            _inertiaApplier = inertiaApplier ?? throw new ArgumentNullException(nameof(inertiaApplier));
            _cameraDragger = cameraDragger ?? throw new ArgumentNullException(nameof(cameraDragger));
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

            Bounds bounds = _areaService.AreaBounds;
            float groundY = _areaService.GroundPlaneY;
            Vector3 targetWorldPoint = ComputeWorldPoint(bounds, groundY, normX, normY);

            Vector3 delta = targetWorldPoint - _frustumService.ProjectedCenter;
            Vector3 newPos = _cameraRig.transform.position + delta;
            Vector3 restrictedPos = _positionRestrictor.Restrict(newPos, _cameraRig.transform.position);
            _cameraRig.transform.position = restrictedPos;

            _frustumService.RefreshNow();
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