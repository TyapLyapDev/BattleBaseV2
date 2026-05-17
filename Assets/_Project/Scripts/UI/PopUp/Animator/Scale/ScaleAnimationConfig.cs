using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    [CreateAssetMenu(
        fileName = nameof(ScaleAnimationConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ScaleAnimationConfig))]
    public class ScaleAnimationConfig : ScriptableObject
    { 
        [SerializeField] private Vector3 _startScale = Vector3.zero;
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        [SerializeField][Min(0)] private float _duration = 0.2f;
        [SerializeField][Min(0)] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.OutBack;

        public Vector3 StartScale => _startScale;

        public Vector3 TargetScale => _targetScale;

        public float Duration => _duration;

        public float Delay => _delay;

        public Ease Ease => _ease;
    }
}