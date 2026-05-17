using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    [CreateAssetMenu(
        fileName = nameof(ColorAnimationConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ColorAnimationConfig))]
    public class ColorAnimationConfig : ScriptableObject
    {
        [SerializeField] private Color _targetColor;
        [SerializeField] private float _duration = 0.7f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        public Color TargetColor => _targetColor;

        public float Duration => _duration;

        public float Delay => _delay;

        public Ease Ease => _ease;
    }
}