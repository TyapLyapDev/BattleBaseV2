using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public class CameraAreaService : ICameraAreaService, IDisposable
    {
        private const float OvershootScaleFactor = 2f;

        private readonly ICameraArea _cameraArea;

        public CameraAreaService(ICameraArea cameraArea)
        {
            _cameraArea = cameraArea ?? throw new ArgumentNullException(nameof(cameraArea));

            _cameraArea.Changed += OnAreaChanged;
            OnAreaChanged();
        }

        public event Action Changed;

        public Bounds AreaBounds { get; private set; }

        public Bounds OvershootBounds { get; private set; }

        public float Resistance { get; private set; }

        public float ResistanceFadeDistance { get; private set; }

        public float GroundPlaneY { get; private set; }

        public void Dispose()
        {
            if (_cameraArea != null)
                _cameraArea.Changed -= OnAreaChanged;
        }

        private void OnAreaChanged()
        {
            Resistance = _cameraArea.Resistance;
            ResistanceFadeDistance = _cameraArea.ResistanceFadeDistance;

            AreaBounds = _cameraArea.Collider.bounds;
            GroundPlaneY = AreaBounds.center.y;
            float fadeDistance = ResistanceFadeDistance * OvershootScaleFactor;
            Vector3 overshootSize = AreaBounds.size + new Vector3(fadeDistance, 0f, fadeDistance);
            OvershootBounds = new Bounds(AreaBounds.center, overshootSize);

            Changed?.Invoke();
        }
    }
}