using UnityEngine;

namespace BattleBase.Gameplay
{
    public class RoadLane : Entity
    {
        [SerializeField] private Color _color;

        private void Awake() =>
            SetColor(_color);
    }
}