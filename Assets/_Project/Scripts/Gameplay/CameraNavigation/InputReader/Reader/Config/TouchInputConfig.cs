using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    [CreateAssetMenu(
        fileName = nameof(TouchInputConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(TouchInputConfig))]
    public class TouchInputConfig : ScriptableObject, ITouchConfig, IClickConfig
    {
        [SerializeField][Min(0.0001f)] private float _zoomSensitivity = 0.002f;
        [SerializeField][Min(0f)] private float _minPinchDistance = 10f;
        [SerializeField][Min(0f)] private float _dragDeltaThreshold = 0.01f;
        [SerializeField][Min(0f)] private float _clickDragThreshold = 5f;

        public float ZoomSensitivity => _zoomSensitivity;

        public float MinPinchDistance => _minPinchDistance;

        public float DragDeltaThreshold => _dragDeltaThreshold;

        public float ClickDragThreshold => _clickDragThreshold;
    }
}