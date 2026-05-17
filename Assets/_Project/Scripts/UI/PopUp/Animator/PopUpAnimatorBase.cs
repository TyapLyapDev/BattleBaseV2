using DG.Tweening;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    public abstract class PopUpAnimatorBase : MonoBehaviour
    {
        public abstract void Init();

        public abstract void HideInstantly();

        public abstract Tweener PlayShow();

        public abstract Tweener PlayHide();
    }
}