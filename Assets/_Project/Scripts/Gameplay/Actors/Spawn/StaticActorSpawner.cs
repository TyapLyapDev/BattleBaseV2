using UnityEngine;

namespace BattleBase.Gameplay.Actors.Spawn
{
    public class StaticActorSpawner : MonoBehaviour
    {
        [SerializeField] private ActorsController _controller;
        [SerializeField] private ActorFactory _factory;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            Actor actor = _factory.Create();

            EstablishTransform(actor.View);

            _controller.AddActor(actor);

            actor.Enable();
            actor.Deactivated += OnDestroyActor;
        }

        private void EstablishTransform(IActorView view)
        {
            if (view is MonoBehaviour actor == false)
                throw new System.InvalidOperationException();

            var unitTransform = actor.transform;
            unitTransform.SetParent(_transform);
            unitTransform.SetPositionAndRotation(_transform.position, _transform.rotation);
            unitTransform.gameObject.SetActive(true);
        }

        private void OnDestroyActor(Actor actor)
        {
            actor.Deactivated -= OnDestroyActor;
            actor.Disable();

            if (actor.View is MonoBehaviour view == false)
                throw new System.InvalidOperationException();

            Destroy(view);
        }
    }
}