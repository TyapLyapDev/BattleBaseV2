using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    public class PopUp : MonoBehaviour
    {
        private readonly List<Tweener> _currentTweens = new(); //todo может обойтись одим _currentSequence

        private List<PopUpAnimatorBase> _popUpAnimators;
        private Sequence _currentSequence;
        private bool _isActive;

        public void Init()
        {
            _popUpAnimators = GetComponents<PopUpAnimatorBase>().ToList();

            foreach (PopUpAnimatorBase animator in _popUpAnimators)
                animator.Init();

            _isActive = gameObject.activeSelf;
        }

        public void Show(Action shownCallback = null)
        {
            if (_isActive)
                return;

            KillCurrentTweens();
            _isActive = true;
            gameObject.SetActive(true);

            foreach (PopUpAnimatorBase animator in _popUpAnimators)
                _currentTweens.Add(animator.PlayShow());

            WaitForAllTweens(_currentTweens, () =>
            {
                _currentTweens.Clear();
                shownCallback?.Invoke();
            });
        }

        public void Hide(Action hiddenCallBack = null)
        {
            if (_isActive == false)
                return;

            KillCurrentTweens();
            _isActive = false;

            foreach (PopUpAnimatorBase animator in _popUpAnimators)
                _currentTweens.Add(animator.PlayHide());

            WaitForAllTweens(_currentTweens, () =>
            {
                _currentTweens.Clear();
                gameObject.SetActive(false);
                hiddenCallBack?.Invoke();
            });
        }

        public void HideInstantly()
        {
            KillCurrentTweens();

            foreach (PopUpAnimatorBase animator in _popUpAnimators)
                animator.HideInstantly();

            gameObject.SetActive(false);
            _isActive = false;
        }

        private void KillCurrentTweens()
        {
            if (_currentSequence != null && _currentSequence.IsActive())
                _currentSequence.Kill();

            _currentSequence = null;

            foreach (Tweener tweener in _currentTweens)
            {
                if (tweener != null && tweener.IsActive())
                    tweener.Kill(false);
            }

            _currentTweens.Clear();
        }

        private void WaitForAllTweens(IEnumerable<Tweener> tweeners, Action completed)
        {
            List<Tweener> list = tweeners.ToList();

            if (list.Count == 0)
            {
                completed?.Invoke();

                return;
            }

            int remaining = list.Count;

            foreach (Tweener tweener in list)
            {
                if (tweener == null || !tweener.IsActive() || tweener.IsComplete())
                {
                    remaining--;

                    continue;
                }

                tweener.OnComplete(() =>
                {
                    remaining--;
                    if (remaining == 0)
                        completed?.Invoke();
                });
            }

            if (remaining == 0)
                completed?.Invoke();
        }
    }
}