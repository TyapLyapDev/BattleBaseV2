using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IClickDetector
    {
        public event Action<Collider> Clicked;
    }
}