using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.UpdateService
{
    public class ActionWrapperRegistry
    {
        private readonly UpdateHandlerCollection _handlerCollection;
        private readonly Dictionary<Action, Dictionary<UpdateType, Action<float>>> _wrappers = new();

        public ActionWrapperRegistry(UpdateHandlerCollection handlerCollection)
        {
            _handlerCollection = handlerCollection;
        }

        public void Subscribe(Action handler, UpdateType updateType)
        {
            if (handler == null)
            {
                Debug.LogError($"{UpdaterConstants.Mark} Attempt to subscribe a null delegate (no parameters)");

                return;
            }

            if (_wrappers.TryGetValue(handler, out var typeMap) == false)
            {
                typeMap = new Dictionary<UpdateType, Action<float>>();
                _wrappers[handler] = typeMap;
            }

            if (typeMap.ContainsKey(updateType))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{UpdaterConstants.Mark} Subscriber \"{handler.Target}.{handler.Method.Name}\" is already subscribed to {updateType} (no deltaTime).");
#endif
                return;
            }

            ActionWrapper wrapper = new(handler, updateType, this);
            Action<float> wrapperDelegate = wrapper.GetDelegate();

            typeMap[updateType] = wrapperDelegate;

            if (_handlerCollection.TryGetList(updateType, out List<Action<float>> list))
                _handlerCollection.Add(list, wrapperDelegate, updateType);
        }

        public void Unsubscribe(Action handler, UpdateType updateType)
        {
            if (handler == null)
            {
                Debug.LogError($"{UpdaterConstants.Mark} Attempt to unsubscribe a null delegate (no parameters)");

                return;
            }

            if (_wrappers.TryGetValue(handler, out var typeMap) &&
                typeMap.TryGetValue(updateType, out var wrapper))
            {
                if (_handlerCollection.TryGetList(updateType, out var list))
                    _handlerCollection.Remove(list, wrapper, updateType);

                typeMap.Remove(updateType);

                if (typeMap.Count == 0)
                    _wrappers.Remove(handler);
            }
        }
    }
}