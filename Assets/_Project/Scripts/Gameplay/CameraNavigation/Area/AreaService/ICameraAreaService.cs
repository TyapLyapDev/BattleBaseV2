using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraAreaService
    {
        public event Action Changed;

        public Bounds AreaBounds { get; }

        public Bounds OvershootBounds { get; }

        public float GroundPlaneY { get; }

        public float Resistance { get; }

        public float ResistanceFadeDistance { get; }
    }
}