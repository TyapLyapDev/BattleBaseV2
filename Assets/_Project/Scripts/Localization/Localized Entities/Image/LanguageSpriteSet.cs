using System;
using UnityEngine;

namespace BattleBase.Localization
{
    [Serializable]
    public class LanguageSpriteSet : ILanguageVisitor
    {
        [SerializeField] private Sprite _ru;
        [SerializeField] private Sprite _en;
        [SerializeField] private Sprite _tr;

        private Sprite _result;

        public Sprite GetByLanguage(ILanguage language)
        {
            language.Accept(this);

            return _result;
        }

        void ILanguageVisitor.Visit(RuLanguage lang) => _result = _ru;

        void ILanguageVisitor.Visit(EnLanguage lang) => _result = _en;

        void ILanguageVisitor.Visit(TrLanguage lang) => _result = _tr;
    }
}