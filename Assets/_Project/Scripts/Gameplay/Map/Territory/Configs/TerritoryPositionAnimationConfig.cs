using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    [CreateAssetMenu(
        fileName = nameof(TerritoryPositionAnimationConfig), 
        menuName = Constants.ConfigsAssetMenuPath + nameof(TerritoryPositionAnimationConfig))]
    public class TerritoryPositionAnimationConfig : ScriptableObject
    {
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.OutBack;

        public float Duration => _duration;

        public float Delay => _delay;

        public Ease Ease => _ease;
    }
}