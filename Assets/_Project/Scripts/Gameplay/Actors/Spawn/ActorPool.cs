using BattleBase.Core;
using System;
using System.Collections.Generic;

namespace BattleBase.Gameplay.Actors.Spawn
{
    public class ActorPool : IPool<Actor>
    {
        private readonly IFactory<Actor> _factory;
        private readonly Stack<Actor> _actors;
        private readonly int _size;

        private int _count;

        public ActorPool(IFactory<Actor> factory, int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _actors = new Stack<Actor>();
            _size = size;
        }

        public bool TryGive(out Actor actor)
        {
            actor = null;

            if (_actors.Count == 0 && _count >= _size)
                return false;

            actor = _actors.Count > 0 ? _actors.Pop() : Create();
            actor.Deactivated += Return;

            return true;
        }

        private void Return(Actor actor)
        {
            if (actor == null)
                return;

            actor.Deactivated -= Return;
            actor.Disable();
            _actors.Push(actor);
        }

        private Actor Create()
        {
            _count++;

            return _factory.Create();
        }
    }
}