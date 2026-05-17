using System.Collections.Generic;
using BattleBase.UI.PopUps;
using UnityEngine;

namespace BattleBase.Commands
{
    public class CommandInitAllPopUps : CommandBase
    {
        [SerializeField] private List<PopUp> _popUps;

        protected override void OnExecute()
        {
            foreach (PopUp popUp in _popUps)
            {
                popUp.Init();
                popUp.HideInstantly();
            }
        }
    }
}