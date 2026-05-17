using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.UpdateService
{
    public class UpdateHandlerCollection
    {
        private readonly Dictionary<UpdateType, List<Action<float>>> _handlers;
        private readonly bool _logSubscribers;

        public UpdateHandlerCollection(bool logSubscribers)
        {
            _logSubscribers = logSubscribers;
            _handlers = new Dictionary<UpdateType, List<Action<float>>>
            {
                [UpdateType.Update] = new List<Action<float>>(),
                [UpdateType.FixedUpdate] = new List<Action<float>>(),
                [UpdateType.LateUpdate] = new List<Action<float>>()
            };
        }

        public bool TryGetList(UpdateType updateType, out List<Action<float>> list)
        {
            if (_handlers.TryGetValue(updateType, out list))
                return true;

            Debug.LogError($"{UpdaterConstants.Mark} Unsupported update type: {updateType}");

            return false;
        }

        public void Add(List<Action<float>> handlers, Action<float> handler, UpdateType updateType)
        {
            if (handler == null)
            {
                Debug.LogError($"{UpdaterConstants.Mark} Attempt to subscribe a null delegate");

                return;
            }

            if (handlers.Contains(handler))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{UpdaterConstants.Mark} Subscriber \"{handler.Target}.{handler.Method.Name}\" is already subscribed to {updateType}");
#endif
                return;
            }

            handlers.Add(handler);
#if UNITY_EDITOR
            if (_logSubscribers)
                Debug.Log($"{UpdaterConstants.Mark} [{updateType}] Subscriber added: \"{handler.Target}\". Total: {handlers.Count}");
#endif
        }

        public void Remove(List<Action<float>> handlers, Action<float> handler, UpdateType updateType)
        {
            if (handlers.Remove(handler))
            {
#if UNITY_EDITOR
                if (_logSubscribers)
                    Debug.Log($"{UpdaterConstants.Mark} [{updateType}] Subscriber removed: \"{handler.Target}\". Remaining: {handlers.Count}");
#endif
            }
        }

        public void InvokeAndCleanup(UpdateType type, float delta)
        {
            List<Action<float>> list = _handlers[type];

            for (int i = list.Count - 1; i >= 0; i--)
            {
                Action<float> handler = list[i];

                if (handler == null)
                {
                    list.RemoveAt(i);

                    continue;
                }

                if (handler.Target is UnityEngine.Object unityObj && unityObj == null)
                {
                    Debug.LogError($"{UpdaterConstants.Mark} Subscriber {handler.Target} was destroyed but never unsubscribed!");
                    list.RemoveAt(i);

                    continue;
                }

                handler.Invoke(delta);
            }
        }

        public string GetDebugString()
        {
            System.Text.StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"Update handlers ({_handlers[UpdateType.Update].Count}):");
            AppendHandlers(stringBuilder, _handlers[UpdateType.Update]);
            stringBuilder.AppendLine($"FixedUpdate handlers ({_handlers[UpdateType.FixedUpdate].Count}):");
            AppendHandlers(stringBuilder, _handlers[UpdateType.FixedUpdate]);
            stringBuilder.AppendLine($"LateUpdate handlers ({_handlers[UpdateType.LateUpdate].Count}):");
            AppendHandlers(stringBuilder, _handlers[UpdateType.LateUpdate]);

            return stringBuilder.ToString();
        }

        private void AppendHandlers(System.Text.StringBuilder stringBuilder, List<Action<float>> handlers)
        {
            foreach (Action<float> handler in handlers)
            {
                string target = handler.Target?.ToString() ?? "static";
                string method = handler.Method.Name;
                stringBuilder.AppendLine($"  - {target}.{method}");
            }
        }
    }
}