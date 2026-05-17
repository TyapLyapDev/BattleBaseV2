using System;
using BattleBase.Gameplay.Map;
using BattleBase.SaveService;
using UnityEngine;

namespace BattleBase.UI.PopUps
{
    public class ColorSettingsPopUp : PopUp
    {
        [SerializeField] private ColorSet _playerColorSet; 
        [SerializeField] private ColorSet _enemyColorSet;

        public event Action Changed;

        public Color PlayerColor => _playerColorSet.CurrentColor;

        public Color EnemyColor => _enemyColorSet.CurrentColor;

        public int PlayerColorIndex => _playerColorSet.CurrentColorIndex;

        public int EnemyColorIndex => _enemyColorSet.CurrentColorIndex;

        private void OnEnable()
        {
            _playerColorSet.Clicked += OnClickPlayerColor;
            _enemyColorSet.Clicked += OnClickEnemyColor;
        }

        private void OnDisable()
        {
            _playerColorSet.Clicked -= OnClickPlayerColor;
            _enemyColorSet.Clicked -= OnClickEnemyColor;
        }

        public void InitColors(IColorData colorData)
        {
            int enemyColorIndex = colorData.EnemyColorIndex;
            int playerColorIndex = colorData.PlayerColorIndex;

            _enemyColorSet.Init(enemyColorIndex);
            _playerColorSet.Init(playerColorIndex);

            _enemyColorSet.EnableInteractableAll();
            _enemyColorSet.DisableInteractable(playerColorIndex);

            _playerColorSet.EnableInteractableAll();
            _playerColorSet.DisableInteractable(enemyColorIndex);
        }

        private void OnClickPlayerColor(int index)
        {
            _enemyColorSet.EnableInteractableAll();
            _enemyColorSet.DisableInteractable(index);

            Changed?.Invoke();
        }

        private void OnClickEnemyColor(int index)
        {
            _playerColorSet.EnableInteractableAll();
            _playerColorSet.DisableInteractable(index);

            Changed?.Invoke();
        }
    }
}