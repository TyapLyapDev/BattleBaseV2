using System.Collections.Generic;
using BattleBase.UI.PopUps;
using UnityEngine;

namespace BattleBase.Commands
{
    public sealed class CommandHidePopUps : CommandBase
    {
        [SerializeField] private List<PopUp> _popUpsToHide;

        public override void Execute()
        {
            foreach (PopUp popUp in _popUpsToHide)
                popUp.Hide();
        }
    }
}