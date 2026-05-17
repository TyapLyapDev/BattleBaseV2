using System;

namespace BattleBase.UpdateService
{
    public class ActionWrapper
    {
        private readonly Action _handler;
        private readonly UpdateType _updateType;
        private readonly ActionWrapperRegistry _registry;

        public ActionWrapper(Action handler, UpdateType updateType, ActionWrapperRegistry registry)
        {
            _handler = handler;
            _updateType = updateType;
            _registry = registry;
        }

        public void Invoke(float dt)
        {
            if (_handler.Target is UnityEngine.Object obj && obj == null)
            {
                _registry.Unsubscribe(_handler, _updateType);

                return;
            }

            _handler();
        }

        public Action<float> GetDelegate() =>
            Invoke;
    }
}