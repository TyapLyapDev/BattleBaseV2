using System;
using BattleBase.Core;
using VContainer;
using VContainer.Unity;

namespace BattleBase.Gameplay.Map
{
    public class TerritorySelectPopUpFactory : IFactory<TerritorySelectPopUp>
    {
        private readonly TerritorySelectPopUp _prefab;
        private readonly IObjectResolver _resolver;

        public TerritorySelectPopUpFactory(
            TerritorySelectPopUp prefab, 
            IObjectResolver resolver)
        {
            _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public TerritorySelectPopUp Create()
        {
            TerritorySelectPopUp popUp = _resolver.Instantiate(_prefab);
            popUp.Init();

            return popUp;
        }
    }
}