using System;
using System.Collections;
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
        private ICameraOrientationAdapter _cameraOrientationAdapter;

        [Inject]
        public void Construct(
            ICameraHandle cameraHandle,
            IScreenOrientationTracker orientationTracker,
            IFrustumProjectionService frustumProjectionService,
            ICameraOrientationAdapter cameraOrientationAdapter)
        {
            _cameraHandle = cameraHandle ?? throw new ArgumentNullException(nameof(cameraHandle));
            _orientationTracker = orientationTracker ?? throw new ArgumentNullException(nameof(orientationTracker));
            _frustumProjectionService = frustumProjectionService ?? throw new ArgumentNullException(nameof(frustumProjectionService));
            _cameraOrientationAdapter = cameraOrientationAdapter ?? throw new ArgumentNullException(nameof(cameraOrientationAdapter));
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
            Vector3 positionToRestore = _frustumProjectionService.Projection.Center;

            bool isPortrait = _orientationTracker.ScreenOrientation == ScreenOrientationType.Portrait;
            _verticalCanvas.SetActive(isPortrait);
            _horizontalCanvas.SetActive(isPortrait == false);

            Transform cameraRig = _cameraHandle.CameraRigTransform;
            Vector3 angles = cameraRig.transform.eulerAngles;
            angles.y = isPortrait ? _verticalCameraRotationY : _horizontalCameraRotationY;
            _cameraHandle.SetCameraRigEulerAngles(angles);
            _cameraOrientationAdapter.Refresh();

            Vector3 currentPosition = _frustumProjectionService.Projection.Center;
            Vector3 delta = currentPosition - positionToRestore;

            _cameraHandle.SetCameraRigPosition(_cameraHandle.CameraRigPosition - delta);
        }
    }
}