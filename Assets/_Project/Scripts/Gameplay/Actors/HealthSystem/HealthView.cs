using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    public class HealthView : MonoBehaviour, IHealthViewComponent
    {
        [SerializeField][SerializeIterface(typeof(IHealthViewComponent))] private GameObject[] _viewComponents;

        private List<IHealthViewComponent> _components;

        public void Init(IHealthEvents healthEvents)
        {
            _components = new List<IHealthViewComponent>();

            foreach (var gameObject in _viewComponents)
            {
                var components = gameObject.GetComponents<IHealthViewComponent>();

                foreach(var component in components)
                    component.Init(healthEvents);

                _components.AddRange(components);
            }
        }
    }
}