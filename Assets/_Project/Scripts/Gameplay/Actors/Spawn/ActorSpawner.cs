using BattleBase.Gameplay.Actors.Movement;
using System.Collections;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Spawn
{
    [RequireComponent(typeof(WaypointController))]
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private ActorsController _controller;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ActorFactory _factory;
        [SerializeField][Min(20)] private int _maxSpawnCount = 50;

        private ActorPool _pool;
        private WaypointController _waypointsController;

        private void OnValidate()
        {
            if (_container == null)
                _container = transform;

            if (_spawnPoint == null)
                _spawnPoint = transform;
        }

        private void Awake()
        {
            _pool = new ActorPool(_factory, _maxSpawnCount);
            _waypointsController = GetComponent<WaypointController>();
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (gameObject.activeSelf)
            {
                if (_pool.TryGive(out Actor actor))
                {
                    actor.Enable();
                    IActorData data = actor.Data;

                    EstablishTransform(actor.View);

                    _controller.AddActor(actor);
                    _waypointsController.SpecifyActorRoute(actor);

                    yield return new WaitForSeconds(data.ConstructionTime);
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private void EstablishTransform(IActorView view)
        {
            if (view is MonoBehaviour actor == false)
                throw new System.InvalidOperationException();

            var unitTransform = actor.transform;
            unitTransform.SetParent(_container);
            unitTransform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            unitTransform.gameObject.SetActive(true);
        }
    }
}
