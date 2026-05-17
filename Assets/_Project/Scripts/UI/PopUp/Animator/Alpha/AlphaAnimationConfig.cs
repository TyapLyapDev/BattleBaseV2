using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    [CreateAssetMenu(
        fileName = nameof(AlphaAnimationConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(AlphaAnimationConfig))] 
    public class AlphaAnimationConfig : ScriptableObject
    {
        [SerializeField][Range(0f, 1f)] private float _startAlpha = 0f;
        [SerializeField][Range(0f, 1f)] private float _targetAlpha = 1f;
        [SerializeField][Min(0)] private float _duration = 0.2f;
        [SerializeField][Min(0)] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.OutBack;

        public float StartAlpha => _startAlpha;

        public float TargetAlpha => _targetAlpha;

        public float Duration => _duration;

        public float Delay => _delay;

        public Ease Ease => _ease;
    }
}