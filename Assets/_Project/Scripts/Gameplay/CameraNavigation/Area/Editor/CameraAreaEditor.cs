#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.Editor
{
    [CustomEditor(typeof(CameraArea))]
    public class CameraAreaEditor : UnityEditor.Editor
    {
        private static readonly Color s_AreaColor = Color.blue;
        private static readonly Color s_OvershootColor = Color.red;
        private static readonly Color s_FrustumColor = Color.yellow;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        public static void DrawCameraAreaGizmos(CameraArea area, GizmoType _)
        {
            if (area == null)
                return;

            if (area.ShouldDrawGizmos == false)
                return;

            ScreenOrientationType screenOrientationType = GetGameViewOrientation();
            GroundProjection areaGround = area.GetAreaGroundProjection(screenOrientationType);
            GroundProjection overshoot = area.GetOvershootGroundProjection(screenOrientationType);

            DrawProjection(areaGround, s_AreaColor);
            DrawProjection(overshoot, s_OvershootColor);

            Camera mainCamera = Camera.main;

            if (mainCamera == null)
                return;

            CameraProjectionType projectionType = mainCamera.orthographic ? CameraProjectionType.Orthographic : CameraProjectionType.Perspective;

            FrustumProjection projection = CameraProjectionUtility.GetFrustumProjection(
                mainCamera,
                area.GroundPlane,
                projectionType);

            GroundProjection frustum = CameraProjectionUtility.ConvertProjection(
                projection,
                area.CameraRig.transform,
                FrustumSizeType.MaximumWidthAndHeight,
                FrustumShape.Trapezoid);

            DrawProjection(frustum, s_FrustumColor);
        }

        private static void DrawProjection(GroundProjection area, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(area.LeftUp, area.RightUp);
            Gizmos.DrawLine(area.RightUp, area.RightDown);
            Gizmos.DrawLine(area.RightDown, area.LeftDown);
            Gizmos.DrawLine(area.LeftDown, area.LeftUp);

            GUIStyle labelStyle = new()
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = color },
                fontSize = 9,
            };

            Handles.color = Color.white;

            DrawLabelWithBackground(area.LeftUp, "Left Up", labelStyle);
            DrawLabelWithBackground(area.RightUp, "Right Up", labelStyle);
            DrawLabelWithBackground(area.LeftDown, "Left Down", labelStyle);
            DrawLabelWithBackground(area.RightDown, "Right Down", labelStyle);
            DrawLabelWithBackground(area.Center, $"Width: {area.Width:F2}\nHeight: {area.Height:F2}", labelStyle);
        }

        private static ScreenOrientationType GetGameViewOrientation()
        {
            Vector2 gameViewSize = Handles.GetMainGameViewSize();

            return gameViewSize.y > gameViewSize.x ? ScreenOrientationType.Portrait : ScreenOrientationType.Landscape;
        }

        private static void DrawLabelWithBackground(Vector3 worldPos, string text, GUIStyle labelStyle)
        {
            Handles.BeginGUI();
            Vector2 screenPos = HandleUtility.WorldToGUIPoint(worldPos);
            Vector2 textSize = labelStyle.CalcSize(new(text));
            Rect rect = new(screenPos.x - textSize.x / 2, screenPos.y - textSize.y / 2, textSize.x, textSize.y);
            Rect bgRect = new(rect.x - 2, rect.y - 2, rect.width + 4, rect.height + 4);

            EditorGUI.DrawRect(bgRect, new Color(0f, 0f, 0f, 0.8f));
            GUI.Label(rect, text, labelStyle);
            Handles.EndGUI();
        }
    }
}
#endif