using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntitySizeTracker : IDisposable
    {
        public event Action Changed;

        public Vector2 WorldSize { get; }
    }
}