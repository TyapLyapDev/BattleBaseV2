using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Movement
{
    public class MoverView : MonoBehaviour, IMoverViewComponent
    {
        [SerializeField][SerializeIterface(typeof(IMoverViewComponent))] private GameObject[] _viewComponents;

        private List<IMoverViewComponent> _components;

        public void Init(IMoverEvents moverEvents)
        {
            _components = new List<IMoverViewComponent>();

            foreach (var gameObject in _viewComponents)
            {
                var components = gameObject.GetComponents<IMoverViewComponent>();

                foreach (var component in components)
                    component.Init(moverEvents);

                _components.AddRange(components);
            }
        }
    }
}
