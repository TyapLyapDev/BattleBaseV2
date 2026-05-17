using System;
using System.Collections.Generic;
using BattleBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    public class TerritoryElevator : ITerritoryElevator, IDisposable
    {
        private Vector3 _distance = new(0, 0.3f, 0);

        private readonly ITerritorySelector _selector;
        private readonly TerritoryPositionAnimationConfig _animationConfig;

        private readonly Dictionary<Territory, Vector3> _originalPositions = new();

        public TerritoryElevator(ITerritorySelector selector, TerritoryPositionAnimationConfig animationConfig)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
            _animationConfig = animationConfig != null ? animationConfig : throw new ArgumentNullException(nameof(animationConfig));

            _selector.Selected += OnTerritorySelected;
            _selector.Unselected += OnTerritoryUnselected;
        }

        public void Dispose()
        {
            if (_selector != null)
            {
                _selector.Selected -= OnTerritorySelected;
                _selector.Unselected -= OnTerritoryUnselected;
            }
        }

        private void OnTerritorySelected(Territory territory)
        {
            Transform territoryTransform = territory.transform;

            if (_originalPositions.ContainsKey(territory) == false)
                _originalPositions[territory] = territoryTransform.localPosition;

            Vector3 originalPos = _originalPositions[territory];
            Vector3 targetPos = originalPos + _distance;
            KillTweens(territoryTransform);
            territoryTransform.PlayLocalMove(targetPos, _animationConfig);
        }

        private void OnTerritoryUnselected(Territory territory)
        {
            if (_originalPositions.TryGetValue(territory, out Vector3 originalPos) == false)
                return;

            Transform territoryTransform = territory.transform;
            KillTweens(territoryTransform);
            territoryTransform.PlayLocalMove(originalPos, _animationConfig);
        }

        private void KillTweens(Transform target)
        {
            if (target == null)
                return;

            string moveId = $"LocalMove_{target.gameObject.GetInstanceID()}";
            DOTween.Kill(moveId);
        }
    }
}