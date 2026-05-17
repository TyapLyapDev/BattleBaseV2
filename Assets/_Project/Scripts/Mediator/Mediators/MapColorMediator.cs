using System;
using BattleBase.DI;
using BattleBase.Gameplay.Map;
using BattleBase.SaveService;
using BattleBase.UI.PopUps;
using UnityEngine;
using VContainer;

namespace BattleBase.Mediators
{
    public class MapColorMediator : MediatorBase, ISaveable, IInjectable
    {
        [SerializeField] private ColorSettingsPopUp _colorSettingsPopUp;
        [SerializeField] private MapTerritoryMediator _territoryMediator;
        [SerializeField] private float _lightenFactor = 0.3f;

        private IColorSaver _saver;

        [Inject]
        public void Construct(IColorSaver saver) =>
            _saver = saver ?? throw new ArgumentNullException(nameof(saver));

        private void OnEnable()
        {
            _colorSettingsPopUp.Changed += OnColorChanged;
            _territoryMediator.Changed += OnTerritoryOwnerChanged;
        }

        private void OnDisable()
        {
            _colorSettingsPopUp.Changed -= OnColorChanged;
            _territoryMediator.Changed -= OnTerritoryOwnerChanged;
        }

        public override void Init() =>
            OnColorChanged();

        public void Load() =>
            _colorSettingsPopUp.InitColors(_saver.ColorData);

        public void Save()
        {
            ColorData data = new(_colorSettingsPopUp.PlayerColorIndex, _colorSettingsPopUp.EnemyColorIndex);
            _saver.SetColorData(data);
        }

        private void OnColorChanged() =>
            UpdateTerritoryColors();

        private void OnTerritoryOwnerChanged() =>
            UpdateTerritoryColors();

        private void UpdateTerritoryColors()
        {
            foreach (Territory territory in _territoryMediator.Territories)
                territory.SetColor(GetColor(territory.Owner));
        }

        private Color GetColor(TerritoryOwnerType owner)
        {
            return owner switch
            {
                TerritoryOwnerType.Enemy => _colorSettingsPopUp.EnemyColor,
                TerritoryOwnerType.Player => _colorSettingsPopUp.PlayerColor,
                TerritoryOwnerType.Contested => Color.Lerp(_colorSettingsPopUp.EnemyColor, Color.white, _lightenFactor),
                _ => throw new ArgumentOutOfRangeException(nameof(owner), owner, "Type is not registered"),
            };
        }
    }
}