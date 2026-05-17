using BattleBase.Gameplay.Actors.Weapons.Missiles;

namespace BattleBase.Gameplay.Actors.Spawn
{
    public interface IMissileSpawner
    {
        public IMissile Spawn(string missileId);
    }
}