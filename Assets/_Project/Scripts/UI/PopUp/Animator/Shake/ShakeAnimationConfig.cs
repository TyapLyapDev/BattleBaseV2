using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    [CreateAssetMenu(
        fileName = nameof(ShakeAnimationConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ShakeAnimationConfig))]
    public class ShakeAnimationConfig : ScriptableObject
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _strength = 5f;
        [SerializeField] private float _rotationStrength = 2.5f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _snapping = false;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _isLoop = true;

        public float Duration => _duration;

        public float Strength => _strength;

        public float RotationStrength => _rotationStrength;

        public int Vibrato => _vibrato;

        public float Randomness => _randomness;

        public bool Snapping => _snapping;

        public Ease Ease => _ease;

        public bool IsLoop => _isLoop;
    }
}