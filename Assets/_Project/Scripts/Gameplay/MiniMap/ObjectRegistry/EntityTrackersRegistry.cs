using System;
using System.Collections.Generic;

namespace BattleBase.Gameplay.MiniMap
{
    public class EntityTrackersRegistry : IEntityTrackersRegistry
    {
        private readonly List<IEntityTracker> _trackers = new();

        public event Action<IEntityTracker> Added;
        public event Action<IEntityTracker> Removed;

        public IReadOnlyList<IEntityTracker> Trackers => _trackers;

        public void Register(IEntityTracker tracker)
        {
            if (tracker == null)
                throw new ArgumentNullException(nameof(tracker));

            if (_trackers.Contains(tracker))
                throw new InvalidOperationException("Attempt to re-register");

            _trackers.Add(tracker);
            tracker.Disposed += Unregister;
            Added?.Invoke(tracker);
        }

        public void Unregister(IEntityTracker tracker)
        {
            if (tracker == null)
                throw new ArgumentNullException(nameof(tracker));

            if (_trackers.Contains(tracker) == false)
                throw new InvalidOperationException("The tracker is missing from the list");

            tracker.Disposed -= Unregister;
            _trackers.Remove(tracker);
            Removed?.Invoke(tracker);
        }
    }
}