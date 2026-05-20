using UnityEngine;

namespace BattleBase.Gameplay
{
    public abstract class Unit : Entity
    {
        [SerializeField] private Color _color;

        private void Awake() =>
            SetColor(_color);
    }
}