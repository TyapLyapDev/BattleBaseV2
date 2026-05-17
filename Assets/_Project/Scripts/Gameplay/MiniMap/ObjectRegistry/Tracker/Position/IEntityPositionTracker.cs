using System;
using UnityEngine;

namespace BattleBase.Gameplay.MiniMap
{
    public interface IEntityPositionTracker : IDisposable
    {
        public event Action Changed;

        public Vector3 WorldPosition { get; }
    }
}