using System;

namespace BattleBase.Gameplay.Actors
{
    [Flags]
    public enum DamageMask
    {
        [Obsolete("Empty mask is not allowed", true)]
        None = 0,

        Infantry = 1 << 0,
        LightVehicle = 1 << 1,
        HeavyVehicle = 1 << 2,
        Air = 1 << 3
    }
}
