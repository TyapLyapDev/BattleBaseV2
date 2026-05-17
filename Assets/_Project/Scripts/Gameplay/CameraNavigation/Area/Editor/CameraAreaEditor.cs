#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using BattleBase.UpdateService;
using BattleBase.UpdateService.Editor;
using UnityEditor;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.Editor
{
    [CustomEditor(typeof(CameraArea))]
    public class CameraAreaEditor : UnityEditor.Editor
    {
        private const float SphereRadius = 0.1f;
        private const int CornerCount = 4;

        private static readonly Color s_AreaColor = Color.blue;
        private static readonly Color s_OvershootColor = Color.red;
        private static readonly Color s_FrustumColor = Color.yellow;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawCameraAreaGizmos(CameraArea area, GizmoType gizmoType)
        {
            if (area == null)
                return;

            if (area.ShouldDrawGizmos == false)
                return;

            Camera mainCamera = Camera.main;

            if (mainCamera == null)
                return;

            IUpdater updater = new EditorUpdater();
            ICameraAreaService areaService = new CameraAreaService(area);
            ICameraTracker cameraTracker = new CameraTracker(mainCamera, updater, area.Config);
            IFrustumProjectionService projectionService = new FrustumProjectionService(mainCamera, areaService, cameraTracker);

            DrawArea(areaService);
            DrawFrustum(projectionService);

            IDisposable[] disposables = new IDisposable[]
            {
                projectionService as IDisposable,
                cameraTracker as IDisposable,
                areaService as IDisposable,
                updater as IDisposable,
            };

            DisposeAll(disposables);
        }

        private static void DisposeAll(params IDisposable[] disposables)
        {
            foreach (IDisposable disposable in disposables)
                disposable?.Dispose();
        }

        private static void DrawArea(ICameraAreaService areaService)
        {
            Bounds bounds = areaService.AreaBounds;
            Bounds overshootBounds = areaService.OvershootBounds;

            Gizmos.color = s_AreaColor;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.color = s_OvershootColor;
            Gizmos.DrawWireCube(overshootBounds.center, overshootBounds.size);
        }

        private static void DrawFrustum(IFrustumProjectionService projectionService)
        {
            IReadOnlyList<Vector3> corners = projectionService.Corners;

            if (corners.Count != CornerCount)
                return;

            Gizmos.color = s_FrustumColor;

            foreach (Vector3 corner in corners)
                Gizmos.DrawWireSphere(corner, SphereRadius);

            for (int i = 0; i < corners.Count; i++)
                Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Count]);
        }
    }
}
#endif