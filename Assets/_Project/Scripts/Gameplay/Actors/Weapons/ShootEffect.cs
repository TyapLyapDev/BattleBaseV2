using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ShootEffect : MonoBehaviour, IWeaponViewComponent
    {
        private IWeaponEvents _weaponEvents;
        private ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            if (_weaponEvents != null)
                _weaponEvents.Shooted += OnPlayShot;
        }

        private void OnDestroy()
        {
            if (_weaponEvents != null)
                _weaponEvents.Shooted -= OnPlayShot;
        }

        public void Init(IWeaponEvents weaponEvents)
        {
            _weaponEvents = weaponEvents ?? throw new System.ArgumentNullException(nameof(weaponEvents));

            if (gameObject.activeSelf)
                _weaponEvents.Shooted += OnPlayShot;
        }

        private void OnPlayShot() => _particle.Play();
    }
}