using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleBase.Localization
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : MonoBehaviour
    {
        [SerializeField] private LanguageSpriteSet _sprites;

        private Image _image;

        private void Awake()
        {
            if (TryGetComponent(out _image) == false)
                throw new NullReferenceException($"Object: {name}, Missing Image");
        }

        private void OnEnable()
        {
            YandexGameLanguageSystemAdapter.Changed += OnSwitchLanguage;
            OnSwitchLanguage();
        }

        private void OnDisable() =>
            YandexGameLanguageSystemAdapter.Changed -= OnSwitchLanguage;

        private void OnSwitchLanguage()
        {
            Sprite sprite = _sprites.GetByLanguage(YandexGameLanguageSystemAdapter.CurrentLanguage);

            if (sprite == null)
                throw new ArgumentNullException($"{nameof(LocalizedImage)} on {name} has no params for current language");

            _image.sprite = sprite;
        }
    }
}