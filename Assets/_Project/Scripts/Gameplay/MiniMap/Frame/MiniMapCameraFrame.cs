using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class MiniMapCameraFrame : MonoBehaviour
    {
        [SerializeField] private RectTransform _frame;

        public void SetSizeWithCurrentAnchors(RectTransform.Axis axis, float size) =>
            _frame.SetSizeWithCurrentAnchors(axis, size);

        public void SetAnchoredPosition(Vector2 position) =>
            _frame.anchoredPosition = position;
    }
}