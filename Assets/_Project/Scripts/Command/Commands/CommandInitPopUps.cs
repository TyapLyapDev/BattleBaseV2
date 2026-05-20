using System.Collections.Generic;
using BattleBase.UI.PopUps;
using UnityEngine;

namespace BattleBase.Commands
{
    public class CommandInitPopUps : CommandBase
    {
        [SerializeField] private List<PopUp> _popUps;

        public override void Execute()
        {
            foreach (PopUp popUp in _popUps)
            {
                popUp.Init();
                popUp.HideInstantly();
            }
        }
    }
}