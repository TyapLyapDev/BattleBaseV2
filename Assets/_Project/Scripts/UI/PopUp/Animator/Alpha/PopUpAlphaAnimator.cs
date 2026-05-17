using System;
using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopUpAlphaAnimator : PopUpAnimatorBase
    {
        [SerializeField] private AlphaAnimationConfig _showConfig; 
        [SerializeField] private AlphaAnimationConfig _hideConfig;

        private CanvasGroup _canvasGroup;

        public override void Init()
        {
            if (_showConfig == null)
                throw new NullReferenceException(nameof(_showConfig)); 

            if (_hideConfig == null)
                throw new NullReferenceException(nameof(_showConfig));

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void HideInstantly() =>
            _canvasGroup.alpha = _showConfig.StartAlpha;

        public override Tweener PlayShow() =>
            _canvasGroup.PlayAlpha(_showConfig);

        public override Tweener PlayHide() =>
            _canvasGroup.PlayAlpha(_hideConfig);
    }
}