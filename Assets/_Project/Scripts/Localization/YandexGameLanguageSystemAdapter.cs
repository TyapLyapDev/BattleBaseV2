using System;
using System.Collections.Generic;
using YG;

namespace BattleBase.Localization
{
    public static class YandexGameLanguageSystemAdapter
    {
        private static readonly LanguageEntry[] _supportedLanguages = new[]
        {
            new LanguageEntry(RuLanguage.Instance, "ru"),
            new LanguageEntry(EnLanguage.Instance, "en"),
            new LanguageEntry(TrLanguage.Instance, "tr"),
        };

        private static readonly List<ILanguage> _languages = new();
        private static readonly Dictionary<ILanguage, string> _languageToCode = new();
        private static readonly Dictionary<string, ILanguage> _codeToLanguage = new();

        static YandexGameLanguageSystemAdapter()
        {
            foreach (LanguageEntry entry in _supportedLanguages)
                Register(entry.Language, entry.Code);

            YG2.onSwitchLang += OnSwitchLang;
            OnSwitchLang(YG2.lang);
        }

        public static event Action Changed;

        public static ILanguage CurrentLanguage { get; private set; }

        public static void Previous() =>
            SwitchRelative(-1);

        public static void Next() =>
            SwitchRelative(1);

        private static void SwitchLanguage(ILanguage language)
        {
            if (_languageToCode.TryGetValue(language, out var langCode) == false)
                throw new Exception($"{nameof(language)} {language} - unregistered type");

            YG2.SwitchLanguage(langCode);
        }

        private static void Register(ILanguage language, string code)
        {
            _languages.Add(language);
            _languageToCode[language] = code;
            _codeToLanguage[code] = language;
        }

        private static void SetLanguage(string langCode)
        {
            CurrentLanguage = _codeToLanguage.GetValueOrDefault(langCode, EnLanguage.Instance);

            Changed?.Invoke();
        }

        private static void SwitchRelative(int delta)
        {
            int index = _languages.IndexOf(CurrentLanguage);

            if (index == -1)
                index = 1;

            int newIndex = (index + delta + _languages.Count) % _languages.Count;
            SwitchLanguage(_languages[newIndex]);
        }

        private static void OnSwitchLang(string lang) =>
            SetLanguage(lang);
    }
}