using System.Collections.Generic;
using BattleBase.UI.PopUps;
using UnityEngine;

namespace BattleBase.Commands
{
    public sealed class CommandShowPopUps : CommandBase
    {
        [SerializeField] private List<PopUp> _popUpsToShow;

        public override void Execute()
        {
            foreach (PopUp popUp in _popUpsToShow)
                popUp.Show();
        }
    }
}