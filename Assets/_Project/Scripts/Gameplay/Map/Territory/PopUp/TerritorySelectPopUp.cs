using System;
using BattleBase.Commands;
using BattleBase.Core;
using BattleBase.Localization;
using BattleBase.UI.Buttons;
using BattleBase.UI.PopUps;
using UnityEngine;
using VContainer;

namespace BattleBase.Gameplay.Map
{
    public class TerritorySelectPopUp : MonoBehaviour, IPoolable<TerritorySelectPopUp>
    {
        [SerializeField] private PopUp _popUp;
        [SerializeField] private ButtonClickHandler _battleButton;
        [SerializeField] private LocalizedText _territoryName;
        [SerializeField] private LocalizedText _ownershipDescriptionText;

        [SerializeField] private LanguageTextsSet _playerOwnership;
        [SerializeField] private LanguageTextsSet _enemyOwnership;
        [SerializeField] private LanguageTextsSet _contestedOwnership;

        private Transform _target;

        public event Action<TerritorySelectPopUp> Deactivated;

        [Inject]
        public void Construct(CommandLoadGameScene commandLoadGameScene, CommandRebuildLayout commandRebuildLayout)
        {
            if (commandLoadGameScene == null)
                throw new ArgumentNullException(nameof(commandLoadGameScene));

            if (commandRebuildLayout == null)
                throw new ArgumentNullException(nameof(commandRebuildLayout));

            _battleButton.AddCommand(commandLoadGameScene);
            commandRebuildLayout.Add(_battleButton.transform as RectTransform);
        }

        private void Update()
        {
            if (_target != null)
                transform.position = _target.position;
        }

        public void Init()
        {
            _popUp.Init();
            _popUp.HideInstantly();
            Update();
        }

        public void SetTarget(Transform target) =>
            _target = target != null ? target : throw new ArgumentNullException(nameof(target));

        public void SetInfo(ITerritoryInfo info) =>
            _territoryName.SetTexts(info.TerritoryName);

        public void SetOwner(TerritoryOwnerType owner)
        {
            LanguageTextsSet texts = owner switch
            {
                TerritoryOwnerType.Player => _playerOwnership,
                TerritoryOwnerType.Enemy => _enemyOwnership,
                TerritoryOwnerType.Contested => _contestedOwnership,
                _ => throw new ArgumentOutOfRangeException(nameof(owner)),
            };

            _ownershipDescriptionText.SetTexts(texts);

            if (owner == TerritoryOwnerType.Enemy)
                HideBattleButton();
            else
                ShowBattleButton();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _popUp.Show();
        }

        public void Hide() =>
            _popUp.Hide(Deactivate);

        public void ShowBattleButton() =>
            _battleButton.Show();

        public void HideBattleButton() =>
            _battleButton.Hide();

        public void Deactivate() =>
            Deactivated?.Invoke(this);
    }
}
