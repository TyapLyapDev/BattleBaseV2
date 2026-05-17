using System.Collections.Generic;
using BattleBase.Commands;
using BattleBase.Mediators;
using UnityEngine;

namespace BattleBase.EntryPoints
{
    public abstract class EntryPointBase : MonoBehaviour
    {
        [SerializeField] private List<MediatorBase> _mediators;
        [SerializeField] private List<CommandBase> _commandsToStart;

        protected virtual void Start()
        {
            foreach (MediatorBase mediator in _mediators)
                mediator.Init();

            foreach (CommandBase command in _commandsToStart)
                command.Execute();
        }
    }
}