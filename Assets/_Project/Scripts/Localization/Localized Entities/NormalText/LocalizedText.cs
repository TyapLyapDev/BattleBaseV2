using System;
using TMPro;
using UnityEngine;

namespace BattleBase.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private LanguageTextsSet _texts;

        private TMP_Text _text;

        private void Awake()
        {
            if (TryGetComponent(out _text) == false)
                throw new NullReferenceException($"Object: {name}, NullComponent: {nameof(_text)}");
        }

        private void OnEnable()
        {
            YandexGameLanguageSystemAdapter.Changed += UpdateInfo;
            UpdateInfo();
        }

        private void OnDisable() =>
            YandexGameLanguageSystemAdapter.Changed -= UpdateInfo;

        public void SetTexts(LanguageTextsSet texts)
        {
            _texts = texts ?? throw new ArgumentNullException(nameof(texts));
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            TextLangParams langParams =
                _texts.GetByLanguage(YandexGameLanguageSystemAdapter.CurrentLanguage)
                ?? throw new ArgumentNullException($"{nameof(LocalizedText)} on {name} has no params for current language");

            _text.text = langParams.Text;
        }
    }
}