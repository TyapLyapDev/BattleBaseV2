using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraArea
    {
        public event Action Changed;

        public CameraRig CameraRig { get; }

        public Bounds AreaBounds { get; }

        public Bounds OvershootBounds { get; }

        public Plane GroundPlane { get; }

        public float Resistance { get; }

        public float GroundPlaneY { get; }

        public GroundProjection GetAreaGroundProjection(ScreenOrientationType orientationType);

        public GroundProjection GetOvershootGroundProjection(ScreenOrientationType orientationType);
    }
}