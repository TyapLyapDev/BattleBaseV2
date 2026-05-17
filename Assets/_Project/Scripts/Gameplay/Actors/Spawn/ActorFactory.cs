using BattleBase.Core;
using BattleBase.Gameplay.Actors.DamageSystem;
using BattleBase.Gameplay.Actors.HealthSystem;
using BattleBase.Gameplay.Actors.Movement;
using BattleBase.Gameplay.Actors.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors.Spawn
{
    public class ActorFactory : MonoBehaviour, IFactory<Actor>
    {
        [SerializeField] private ActorConfig _config;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TeamType _team;

        private ComponentFactoryRegistry _componentFactoryRegistry;
        private ActorBinderRegistry _actorBinderRegistry;
        private int _spawnedUnits;

        private void OnValidate()
        {
            if (TryGetComponent(out Renderer renderer))
                _renderer = renderer;
        }

        private void Awake()
        {
            _spawnedUnits = 0;

            //todo Add to Containers
            _componentFactoryRegistry = CreateComponentFactoryRegistry();
            _actorBinderRegistry = CreateActorBinderRegistry();
        }

        public Actor Create()
        {
            ActorView prefab = _config.Prefab;
            string name = $"{_team}_{prefab.name}_{++_spawnedUnits}";

            ActorView view = Instantiate(prefab, transform);
            view.gameObject.name = name;
            view.Init();

            ActorDataModifier actorDataModified = new(_config.Data, _team);
            ActorBuilder builder = new();
            builder
                .ActorView(view)
                .ActorData(actorDataModified);

            IEnumerable<IComponentSource> componentSources = _config.GetComponentSources();

            foreach (var componentSource in componentSources)
            {
                IActorComponent component = _componentFactoryRegistry.Create(componentSource);
                builder.AddComponent(component);

                if (component is IDamagebleEvents damagebleEvents)
                    builder.DamagebleEvents(damagebleEvents);
            }

            Actor actor = builder.Build();

            _actorBinderRegistry.Bind(actor, view);

            //todo Delete after the selected color is implemented ...
            var renderersActor = GetComponentsInChildren<MeshRenderer>(true);

            foreach (var renderer in renderersActor)
                renderer.material = _renderer.material;
            //...

            return actor;
        }

        private ComponentFactoryRegistry CreateComponentFactoryRegistry()
        {
            return new ComponentFactoryRegistry(new List<IComponentFactory>()
            {
                new HealthFactory(),
                new WeaponFactory(),
                new MoverFactory()
            });
        }

        private ActorBinderRegistry CreateActorBinderRegistry()
        {
            return new(new List<IActorComponentBinder>()
            {
                new HealthBinder(),
                new WeaponBinder(),
                new MoverBinder()
            });
        }
    }
}