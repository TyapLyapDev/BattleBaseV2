using System;
using BattleBase.Gameplay.CameraNavigation;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public class MiniMapArea : MonoBehaviour
    {
        [SerializeField] private RectTransform _area;
        [SerializeField] private ScreenOrientationType _orientation;

        public event Action SizeChanged;

        public Rect Rect => _area.rect;

        public ScreenOrientationType Orientation => _orientation;

        public void SetSizeWithCurrentAnchors(Vector2 size)
        {
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            SizeChanged?.Invoke();
        }
    }
}