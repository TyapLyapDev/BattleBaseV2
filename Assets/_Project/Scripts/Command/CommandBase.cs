using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Commands
{
    public abstract class CommandBase : MonoBehaviour
    {
        [SerializeField] private List<CommandBase> _commands;

        public void Execute()
        {
            foreach (CommandBase command in _commands)
            {
                if (command != this)
                    command.Execute();

            }

            OnExecute();
        }

        protected abstract void OnExecute();
    }
}