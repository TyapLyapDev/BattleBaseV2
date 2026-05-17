using System;
using UnityEngine;

namespace BattleBase.Localization
{
    [Serializable]
    public class TextLangParams
    {
        [SerializeField][TextArea] private string _text = "text";

        public string Text => _text;
    }
}