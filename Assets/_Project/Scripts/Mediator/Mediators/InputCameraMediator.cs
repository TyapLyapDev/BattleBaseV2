using System;
using System.Collections.Generic;
using BattleBase.DI;
using BattleBase.Gameplay.CameraNavigation;
using BattleBase.Gameplay.CameraNavigation.InputReader;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static UnityEngine.Rendering.DebugUI;

namespace BattleBase.Mediators
{
    public class InputCameraMediator : MediatorBase, IInjectable
    {
        [SerializeField] private List<Canvas> _canvasList;
        [SerializeField] private List<Slider> _zoomSliders;
        [SerializeField] private bool _isDynamicAngle;

        private ICameraInputReader _inputReader;
        private ICameraDragger _dragger;
        private ICameraZoom _zoom;
        private IVerticalFactorCalculator _angleCompensator;
        private IUIPointerChecker _pointerChecker;
        private float _dragVerticalFactor;

        [Inject]
        public void Construct(
            ICameraInputReader inputReader,
            ICameraDragger dragger,
            ICameraZoom zoom,
            IVerticalFactorCalculator angleCompensator,
            IUIPointerChecker pointerChecker)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _dragger = dragger ?? throw new ArgumentNullException(nameof(dragger));
            _angleCompensator = angleCompensator ?? throw new ArgumentNullException(nameof(angleCompensator));
            _zoom = zoom ?? throw new ArgumentNullException(nameof(zoom));
            _pointerChecker = pointerChecker ?? throw new ArgumentNullException(nameof(pointerChecker));
        }

        private void OnEnable()
        {
            foreach (Slider zoom in _zoomSliders)
                zoom.onValueChanged.AddListener(OnSliderChanged);
        }

        private void OnDisable()
        {
            foreach (Slider zoom in _zoomSliders)
                zoom.onValueChanged.RemoveListener(OnSliderChanged);
        }

        public override void Init()
        {
            foreach (Canvas canvas in _canvasList)
                _pointerChecker.AddCanvas(canvas);

            UpdateCompensation();

            foreach (Slider zoom in _zoomSliders)
                zoom.value = _zoom.Value01;
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;
            Vector3? dragDelta = _inputReader?.WorldDragDelta;

            if (dragDelta.HasValue)
            {
                Vector3 delta = dragDelta.Value;

                if (_isDynamicAngle)
                    UpdateCompensation();

                delta.z *= _dragVerticalFactor;
                dragDelta = delta;
            }

            _dragger.Update(deltaTime, dragDelta);

            float? zoomDelta = _inputReader?.ZoomDelta;

            _zoom.Update(_inputReader?.ZoomDelta);

            if (zoomDelta.HasValue)
            {
                foreach (Slider zoom in _zoomSliders)
                    zoom.value = _zoom.Value01;
            }
        }

        private void UpdateCompensation() =>
            _dragVerticalFactor = _angleCompensator.CalculateVerticalFactor();

        private void OnSliderChanged(float value)
        {
            if (_inputReader?.ZoomDelta.HasValue == false)
                _zoom.SetValue01(value);
        }
    }
}