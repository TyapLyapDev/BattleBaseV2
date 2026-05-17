using System;
using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation
{
    public interface ICameraArea
    {
        public event Action Changed;

        public BoxCollider Collider { get; }

        public float Resistance { get; }

        public float ResistanceFadeDistance { get; }
    }
}