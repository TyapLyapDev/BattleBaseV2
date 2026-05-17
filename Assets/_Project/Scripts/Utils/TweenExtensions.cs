using System;
using System.Collections.Generic;
using System.Linq;
using BattleBase.Gameplay.Map;
using BattleBase.UI.PopUps;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleBase.Utils
{
    public static class TweenExtensions
    {
        private const string ScaleId = "Scale";
        private const string AlphaId = "Alpha";
        private const string ShakePositionId = "ShakePosition";
        private const string ShakeRotationId = "ShakeRotation";
        private const string ImageColorId = "ImageColor";
        private const string TextColorId = "TextColor";

        public static Tweener PlayScale(this Transform target, ScaleAnimationConfig config)
        {
            if (target == null || config == null)
                return null;

            string id = $"{ScaleId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(id);

            Tweener tweener = target.DOScale(config.TargetScale, config.Duration)
                .SetDelay(config.Delay, true)
                .SetEase(config.Ease)
                .SetId(id)
                .SetLink(target.gameObject)
                .SetUpdate(true);

            return tweener;
        }

        public static Tweener PlayAlpha(this CanvasGroup target, AlphaAnimationConfig config)
        {
            if (target == null || config == null)
                return null;

            string id = $"{AlphaId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(id);

            Tweener tweener = target.DOFade(config.TargetAlpha, config.Duration)
                .SetDelay(config.Delay, true)
                .SetEase(config.Ease)
                .SetId(id)
                .SetLink(target.gameObject)
                .SetUpdate(true);

            return tweener;
        }

        public static Tweener PlayShake(this Transform target, ShakeAnimationConfig config)
        {
            if (target == null || config == null)
                return null;

            string positionId = $"{ShakePositionId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(positionId);

            string rotationId = $"{ShakeRotationId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(rotationId);

            Tweener posTweener = target.DOShakePosition(
                config.Duration,
                config.Strength,
                config.Vibrato,
                config.Randomness,
                config.Snapping,
                false)
                .SetEase(config.Ease)
                .SetId(positionId)
                .SetLink(target.gameObject)
                .SetUpdate(true);

            Tweener rotTweener = target.DOShakeRotation(
                config.Duration,
                config.RotationStrength,
                config.Vibrato,
                config.Randomness,
                false)
                .SetEase(config.Ease)
                .SetId(rotationId)
                .SetLink(target.gameObject)
                .SetUpdate(true);

            if (config.IsLoop)
            {
                posTweener.SetLoops(-1, LoopType.Restart);
                rotTweener.SetLoops(-1, LoopType.Restart);
            }

            return posTweener;
        }

        public static void StopShake(this Transform target)
        {
            if (target == null)
                return;

            string positionId = $"{ShakePositionId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(positionId);

            string rotationId = $"{ShakeRotationId}_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(rotationId);
        }

        public static Tweener PlayColor(this Image image, ColorAnimationConfig config)
        {
            if (image == null || config == null)
                return null;

            string id = $"{ImageColorId}_{image.gameObject.GetInstanceID()}";
            DOTween.Kill(id);

            Tweener tweener = image.DOColor(config.TargetColor, config.Duration)
                .SetDelay(config.Delay, true)
                .SetEase(config.Ease)
                .SetId(id)
                .SetLink(image.gameObject)
                .SetUpdate(true);

            return tweener;
        }

        public static Tweener PlayColor(this TMP_Text text, ColorAnimationConfig config)
        {
            if (text == null || config == null)
                return null;

            string id = $"{TextColorId}_{text.gameObject.GetInstanceID()}";
            DOTween.Kill(id);

            Tweener tweener = text.DOColor(config.TargetColor, config.Duration)
                .SetDelay(config.Delay, true)
                .SetEase(config.Ease)
                .SetId(id)
                .SetLink(text.gameObject)
                .SetUpdate(true);

            return tweener;
        }

        public static void PlaySequence(IEnumerable<Tweener> tweeners, Action onComplete = null)
        {
            if (tweeners == null)
                return;

            List<Tweener> tweenerList = tweeners as List<Tweener> ?? tweeners.ToList();

            if (tweenerList.Count == 0)
            {
                onComplete?.Invoke();

                return;
            }

            Sequence sequence = DOTween.Sequence().SetUpdate(true);

            foreach (Tweener tweener in tweenerList)
                sequence.Join(tweener);

            sequence.OnComplete(() => onComplete?.Invoke());
        }

        public static Tweener PlayLocalMove(this Transform target, Vector3 targetLocalPosition, TerritoryPositionAnimationConfig config)
        {
            if (target == null || config == null)
                return null;

            string id = $"LocalMove_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(id);

            Tweener tweener = target.DOLocalMove(targetLocalPosition, config.Duration)
                .SetDelay(config.Delay, true)
                .SetEase(config.Ease)
                .SetId(id)
                .SetLink(target.gameObject)
                .SetUpdate(true);

            return tweener;
        }
    }
}