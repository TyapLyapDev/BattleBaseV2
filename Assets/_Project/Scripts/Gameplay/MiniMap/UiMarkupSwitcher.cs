using System;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.MiniMap
{
    public class UiMarkupSwitcher : MonoBehaviour, IInjectable
    {
        [SerializeField] private GameObject _verticalCanvas;
        [SerializeField] private GameObject _horizontalCanvas;
        [SerializeField] private float _verticalCameraRotationY;
        [SerializeField] private float _horizontalCameraRotationY;

        private ICameraHandle _cameraHandle;
        private IScreenOrientationTracker _orientationTracker;
        private IFrustumProjectionService _frustumProjectionService;

        [Inject]
        public void Construct(
            ICameraHandle cameraHandle,
            IScreenOrientationTracker orientationTracker,
            IFrustumProjectionService frustumProjectionService)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _orientationTracker = orientationTracker ?? throw new ArgumentNullException(nameof(orientationTracker));
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));
        }

        private void OnEnable()
        {
            _orientationTracker.OrientationChanged += OnOrientationChanged;
            OnOrientationChanged();
        }

        private void OnDisable() =>
            _orientationTracker.OrientationChanged -= OnOrientationChanged;

        private void OnOrientationChanged()
        {
            Vector3 oldCenter = _frustumProjectionService.Projection.Center;
            bool isPortrait = _orientationTracker.ScreenOrientation == ScreenOrientationType.Portrait;
            _verticalCanvas.SetActive(isPortrait);
            _horizontalCanvas.SetActive(isPortrait == false);
            Vector3 angles = _cameraHandle.CameraRig.transform.eulerAngles;
            angles.y = isPortrait ? _verticalCameraRotationY : _horizontalCameraRotationY;
            _cameraHandle.SetCameraRigEulerAngles(angles);
            _frustumProjectionService.Refresh();
            Vector3 newCenter = _frustumProjectionService.Projection.Center;
            Vector3 delta = oldCenter - newCenter;
            _cameraHandle.SetCameraRigPosition(_cameraHandle.CameraRig.transform.position + delta);
        }
    }
}