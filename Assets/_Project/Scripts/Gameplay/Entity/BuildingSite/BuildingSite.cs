using System;
using UnityEngine;

namespace BattleBase.Gameplay
{
    public class BuildingSite : Entity, IBuildingSite
    {
        [SerializeField] private Transform _unitSpawnPoint;
        [SerializeField] private BuildingSiteState _state;
        [SerializeField] private Color _color;

        public event Action StateChanged;

        public BuildingSiteState State => _state;

        public Transform UnitSpawnPoint => _unitSpawnPoint;

        private void Awake() =>
            SetColor(_color);

        public bool TrySelect()
        {
            if (IsPlayer && _state == BuildingSiteState.Active)
            {
                _state = BuildingSiteState.Selected;
                StateChanged?.Invoke();

                return true;
            }

            return false;
        }

        public void Unselect()
        {
            if (IsPlayer && _state == BuildingSiteState.Selected)
            {
                _state = BuildingSiteState.Active;
                StateChanged?.Invoke();
            }
        }

        public void SetInactiveState()
        {
            _state = BuildingSiteState.Inactive;
            StateChanged?.Invoke();
        }
    }
}