using System;
using BattleBase.Core;

namespace BattleBase.Gameplay.MiniMap
{
    public class IconMapObjectFactory : IFactory<IconMapObject>
    {
        private readonly IconMapObject _prefab;

        public IconMapObjectFactory(IconMapObject prefab)
        {
            _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
        }

        public IconMapObject Create()
        {
            IconMapObject icon = UnityEngine.Object.Instantiate(_prefab);

            return icon;
        }
    }
}