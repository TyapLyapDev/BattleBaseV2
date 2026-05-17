#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using BattleBase.UpdateService;
using UnityEditor;

namespace BattleBase.UpdateService.Editor
{
    public class EditorUpdater : IUpdater, IDisposable
    {
        private readonly Dictionary<Action, UpdateType> _updateActions = new Dictionary<Action, UpdateType>();
        private readonly Dictionary<Action<float>, UpdateType> _updateFloatActions = new Dictionary<Action<float>, UpdateType>();

        public EditorUpdater()
        {
            EditorApplication.update += OnUpdate;
        }

        public void Dispose()
        {
            EditorApplication.update -= OnUpdate;
            _updateActions.Clear();
            _updateFloatActions.Clear();
        }

        public IUpdater Subscribe(Action action, UpdateType updateType)
        {
            if (action == null) 
                throw new ArgumentNullException(nameof(action));

            _updateActions[action] = updateType;

            return this;
        }

        public IUpdater Subscribe(Action<float> action, UpdateType updateType)
        {
            if (action == null) 
                throw new ArgumentNullException(nameof(action));

            _updateFloatActions[action] = updateType;

            return this;
        }

        public IUpdater Unsubscribe(Action action, UpdateType updateType)
        {
            if (action != null) 
                _updateActions.Remove(action);

            return this;
        }

        public IUpdater Unsubscribe(Action<float> action, UpdateType updateType)
        {
            if (action != null) 
                _updateFloatActions.Remove(action);

            return this;
        }

        public IUpdater DebugPrint() =>
            this;

        private void OnUpdate()
        {
            foreach (var kvp in _updateActions)
            {
                if (kvp.Value == UpdateType.Update)
                    kvp.Key?.Invoke();
            }

            float deltaTime = (float)EditorApplication.timeSinceStartup - (float)EditorApplication.timeSinceStartup;

            foreach (var kvp in _updateFloatActions)
            {
                if (kvp.Value == UpdateType.Update)
                    kvp.Key?.Invoke(deltaTime);
            }
        }
    }
}
#endif