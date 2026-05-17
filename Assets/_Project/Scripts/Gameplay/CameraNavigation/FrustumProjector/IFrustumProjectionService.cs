using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface IFrustumProjectionService
    {
        public event Action Changed;

        public Vector3 ProjectedCenter { get; }

        public IReadOnlyList<Vector3> Corners { get; }

        public float CachedHeight { get; }

        public float CachedWidth { get; }

        public void RefreshNow();

        public void ProjectCornersOntoPlaneFromPosition(Vector3 cameraPosition, List<Vector3> outCorners);
    }
}