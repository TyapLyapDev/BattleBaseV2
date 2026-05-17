using System;
using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    public class PopUpScaleAnimator : PopUpAnimatorBase
    {
        [SerializeField] private ScaleAnimationConfig _showConfig;
        [SerializeField] private ScaleAnimationConfig _hideConfig; 

        private Transform _transform;

        public override void Init()
        {
            if (_showConfig == null)
                throw new NullReferenceException(nameof(_showConfig));

            if (_hideConfig == null)
                throw new NullReferenceException(nameof(_showConfig));

            _transform = transform;
        }

        public override void HideInstantly() =>
            _transform.localScale = _showConfig.StartScale;

        public override Tweener PlayShow() =>
            _transform.PlayScale(_showConfig);

        public override Tweener PlayHide() =>
            _transform.PlayScale(_hideConfig);
    }
}