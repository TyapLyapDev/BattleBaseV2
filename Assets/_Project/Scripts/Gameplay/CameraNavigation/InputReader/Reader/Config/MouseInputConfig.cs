using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    [CreateAssetMenu(
        fileName = nameof(MouseInputConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(MouseInputConfig))]
    public class MouseInputConfig : ScriptableObject, IZoomConfig, IDragConfig, IClickConfig
    {
        [SerializeField][Min(0.001f)] private float _keyboardSpeed = 1.4f;
        [SerializeField][Min(0.001f)] private float _scrollSensitivity = 1f;
        [SerializeField][Min(0.001f)] private float _dragDeltaThreshold = 0.01f;
        [SerializeField][Min(0.001f)] private float _keyboardAxisThreshold = 0.01f;
        [SerializeField][Min(0.0001f)] private float _scrollThreshold = 0.001f;
        [SerializeField][Min(0f)] private float _clickDragThreshold = 5f;

        public float KeyboardSpeed => _keyboardSpeed;

        public float ScrollSensitivity => _scrollSensitivity;

        public float DragDeltaThreshold => _dragDeltaThreshold;

        public float KeyboardAxisThreshold => _keyboardAxisThreshold;

        public float ScrollThreshold => _scrollThreshold;

        public float ClickDragThreshold => _clickDragThreshold;
    }
}