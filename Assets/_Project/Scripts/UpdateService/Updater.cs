using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.UpdateService
{
    public class Updater : MonoBehaviour, IUpdater
    {
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _logSubscribers; 
#else
        private bool _logSubscribers = false;
#endif

        private UpdateHandlerCollection _handlerCollection;
        private ActionWrapperRegistry _wrapperRegistry;

        private void Awake()
        {
            _handlerCollection = new UpdateHandlerCollection(_logSubscribers);
            _wrapperRegistry = new ActionWrapperRegistry(_handlerCollection);
        }

        private void Update() =>
            _handlerCollection.InvokeAndCleanup(UpdateType.Update, Time.deltaTime);

        private void FixedUpdate() =>
            _handlerCollection.InvokeAndCleanup(UpdateType.FixedUpdate, Time.fixedDeltaTime);

        private void LateUpdate() =>
            _handlerCollection.InvokeAndCleanup(UpdateType.LateUpdate, Time.deltaTime);

        public IUpdater Subscribe(Action<float> handler, UpdateType updateType)
        {
            if (_handlerCollection.TryGetList(updateType, out var list))
                _handlerCollection.Add(list, handler, updateType);

            return this;
        }

        public IUpdater Subscribe(Action handler, UpdateType updateType)
        {
            _wrapperRegistry.Subscribe(handler, updateType);

            return this;
        }

        public IUpdater Unsubscribe(Action<float> handler, UpdateType updateType)
        {
            if (_handlerCollection.TryGetList(updateType, out List<Action<float>> list))
                _handlerCollection.Remove(list, handler, updateType);

            return this;
        }

        public IUpdater Unsubscribe(Action handler, UpdateType updateType)
        {
            _wrapperRegistry.Unsubscribe(handler, updateType);

            return this;
        }

        public IUpdater DebugPrint()
        {
            Debug.Log(_handlerCollection.GetDebugString());

            return this;
        }
    }
}