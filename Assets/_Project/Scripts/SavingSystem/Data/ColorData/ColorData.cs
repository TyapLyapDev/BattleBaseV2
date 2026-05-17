using System;
using UnityEngine;

namespace BattleBase.SaveService
{
    [Serializable]
    public class ColorData : IColorData
    {
        [SerializeField] private int _playerColorIndex = 0;
        [SerializeField] private int _enemyColorIndex = 5;

        public ColorData() { }

        public ColorData(int playerColorIndex, int enemyColorIndex)
        {
            _playerColorIndex = playerColorIndex;
            _enemyColorIndex = enemyColorIndex;
        }

        public int PlayerColorIndex => _playerColorIndex;

        public int EnemyColorIndex => _enemyColorIndex;

        public void SetData(IColorData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _playerColorIndex = data.PlayerColorIndex;
            _enemyColorIndex = data.EnemyColorIndex;
        }
    }
}