using BattleBase.Gameplay.Actors.DamageSystem;
using System;
using System.Collections;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Weapons
{
    public class TargetFinder : MonoBehaviour, ITargetFinder
    {
        [SerializeField] private LayerMask _findedLayerMask;
        [SerializeField][Min(0.1f)] private float _timeUpdate = 0.5f;
        [SerializeField][Range(5, 15)] private int _maxFindedUnits = 10;

        [Header("Debug")]
        [SerializeField] private bool _isDebugEnable;

        private IWeaponPresenter _presenter;
        private IWeaponConfig _weaponConfig;
        private ITeamable _teamable;
        private Transform _transform;
        private Collider[] _foundUnits;
        private Coroutine _coroutine;
        private WaitForSeconds _tick;

        private void Awake()
        {
            _transform = transform;
            _tick = new WaitForSeconds(_timeUpdate);
            _foundUnits = new Collider[_maxFindedUnits];
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(Activate());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        private void OnDrawGizmosSelected()
        {
            if (_isDebugEnable == false || gameObject.activeSelf == false || _weaponConfig == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _weaponConfig.FiringRange);
        }

        public void Init(IWeaponPresenter presenter, IWeaponConfig weaponConfig, ITeamable teamable)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _weaponConfig = weaponConfig ?? throw new ArgumentNullException(nameof(weaponConfig));
            _teamable = teamable ?? throw new ArgumentNullException(nameof(teamable));
        }

        private IEnumerator Activate()
        {
            while (gameObject.activeSelf)
            {
                if (_weaponConfig == null)
                    yield return null;

                if (TryFindEnemyUnit(out ITarget enemy))
                    _presenter.SpecifyTarget(enemy);

                yield return _tick;
            }
        }

        private bool TryFindEnemyUnit(out ITarget enemy)
        {
            enemy = null;

            int count = Physics.OverlapSphereNonAlloc(
                _transform.position,
                _weaponConfig.FiringRange,
                _foundUnits,
                _findedLayerMask,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider collider = _foundUnits[i];

                if (collider.TryGetComponent(out enemy))
                {
                    if (_teamable.TeamType != enemy.TeamType)
                        return true;
                }
            }

            return false;
        }
    }
}
