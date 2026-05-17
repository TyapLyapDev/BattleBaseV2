using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class WeaponView : MonoBehaviour, IWeaponViewComponent
    {
        [SerializeField][SerializeIterface(typeof(IWeaponViewComponent))] private GameObject[] _viewComponents;

        private List<IWeaponViewComponent> _components;

        public void Init(IWeaponEvents weaponEvents)
        {
            _components = new List<IWeaponViewComponent>();

            foreach (var gameObject in _viewComponents)
            {
                var components = gameObject.GetComponents<IWeaponViewComponent>();

                foreach (var component in components)
                    component.Init(weaponEvents);

                _components.AddRange(components);
            }
        }
    }
}