using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Commands
{
    public sealed class CommandSet : CommandBase
    {
        [SerializeField] private List<CommandBase> _commands;

        public override void Execute()
        {
            foreach (CommandBase command in _commands)
            {
                if (command != this)
                    command.Execute();
            }
        }
    }
}