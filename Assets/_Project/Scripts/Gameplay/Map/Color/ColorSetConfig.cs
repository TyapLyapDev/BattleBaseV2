using System.Collections.Generic;
using BattleBase.Utils;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    [CreateAssetMenu(
        fileName = nameof(ColorSetConfig),
        menuName = Constants.ConfigsAssetMenuPath + nameof(ColorSetConfig))]
    public class ColorSetConfig : ScriptableObject
    {
        [SerializeField] private List<Color> _colors;

        public IReadOnlyList<Color> Colors => _colors;
    }
}