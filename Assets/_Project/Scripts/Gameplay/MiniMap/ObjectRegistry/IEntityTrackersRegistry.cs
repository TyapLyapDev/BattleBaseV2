using System;
using System.Collections.Generic;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntityTrackersRegistry
    {
        public event Action<IEntityTracker> Added;
        public event Action<IEntityTracker> Removed;

        public IReadOnlyList<IEntityTracker> Trackers { get; }

        public void Register(IEntityTracker trackable);

        public void Unregister(IEntityTracker trackable);
    }
}