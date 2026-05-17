using BattleBase.Core;
using BattleBase.Gameplay.Actors.Weapons.Missiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Spawn
{
    public class MissileSpawner : MonoBehaviour, IMissileSpawner
    {
        [SerializeField] private List<MissileFactory> _factories;

        private Dictionary<string, Pool<Missile>> _pools;
        private Dictionary<string, Transform> _containers;

        private void Awake()
        {
            _pools = new Dictionary<string, Pool<Missile>>();
            _containers = new Dictionary<string, Transform>();

            foreach (var factory in _factories)
            {
                _containers.Add(factory.MissileId, factory.transform);

                Pool<Missile> pool = new(factory);
                _pools.Add(factory.MissileId, pool);
            }
        }

        public IMissile Spawn(string missileId)
        {
            if (string.IsNullOrEmpty(missileId))
                throw new ArgumentException($"{nameof(missileId)} cannot be null or empty");

            if (_pools.ContainsKey(missileId) == false)
                throw new InvalidOperationException($"{_pools} don't contains key '{missileId}'");

            var pool = _pools[missileId];

            if (pool.TryGive(out Missile missile) == false)
                throw new InvalidOperationException($"There are too many objects, expand the pool");

            missile.gameObject.SetActive(true);
            missile.transform.SetParent(_containers[missileId]);

            return missile;
        }
    }
}