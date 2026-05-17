using System.Collections.Generic;
using BattleBase.UI.PopUps;
using UnityEngine;

namespace BattleBase.Commands
{
    public sealed class CommandShowHidePopUps : CommandBase
    {
        [SerializeField] private List<PopUp> _popUpsToHide;
        [SerializeField] private List<PopUp> _popUpsToShow;

        protected override void OnExecute()
        {
            foreach (PopUp popUp in _popUpsToHide)
                popUp.Hide();

            foreach (PopUp popUp in _popUpsToShow)
                popUp.Show();
        }
    }
}