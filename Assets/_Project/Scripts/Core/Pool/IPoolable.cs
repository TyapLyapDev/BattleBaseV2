using System;

namespace BattleBase.Core
{
    public interface IPoolable<T> 
    {
        public event Action<T> Deactivated;
    }
}