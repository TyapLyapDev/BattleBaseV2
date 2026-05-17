using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleBase.Gameplay.Actors
{
    public class ComponentFactoryRegistry : IComponentFactoryRegistry
    {
        private readonly Dictionary<Type, IComponentFactory> _factories;

        public ComponentFactoryRegistry(IEnumerable<IComponentFactory> factories)
        {
            _factories = factories.ToDictionary(x => x.SourceType);
        }

        public IActorComponent Create(IComponentSource source)
        {
            Type sourceType = source.GetType();

            if (_factories.TryGetValue(sourceType, out IComponentFactory factory) == false)
                throw new InvalidOperationException($"Factory not found for {sourceType}");

            return factory.Create(source);
        }
    }
}
