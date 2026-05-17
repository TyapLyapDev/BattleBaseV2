using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors 
{
    public class ActorView : MonoBehaviour, IActorView
    {
        [SerializeField][SerializeIterface(typeof(IActorViewComponent))] private GameObject[] _viewComponents;

        private Dictionary<Type, IActorViewComponent> _components;

        public void Init()
        {
            _components = new Dictionary<Type, IActorViewComponent>();

            AddActorViewComponents(gameObject.GetComponents<IActorViewComponent>());

            foreach (var gameObject in _viewComponents)
                AddActorViewComponents(gameObject.GetComponents<IActorViewComponent>());
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);


        public bool TryGetViewComponent<T>(out T component) where T : class, IActorViewComponent
        {
            if (_components.TryGetValue(typeof(T), out var value))
            {
                component = (T)value;

                return true;
            }

            component = null;

            return false;
        }

        private void AddActorViewComponents(IActorViewComponent[] components)
        {
            foreach (var component in components)
            {
                Type heir = FindHeir(component);

                _components[heir] = component;
            }
        }

        private Type FindHeir(IActorViewComponent component)
        {
            var interfaces = component.GetType().GetInterfaces();

            foreach (var interfaceType in interfaces)
            {
                if (interfaceType == typeof(IActorViewComponent))
                    continue;

                if (typeof(IActorViewComponent).IsAssignableFrom(interfaceType))
                    return interfaceType;
            }

            return typeof(IActorViewComponent);
        }
    }
}
