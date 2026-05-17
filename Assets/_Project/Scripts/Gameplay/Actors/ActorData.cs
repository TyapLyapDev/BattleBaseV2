using BattleBase.Localization;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    [System.Serializable]
    public class ActorData : IActorData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private LanguageTextsSet _name;
        [SerializeField] private LanguageTextsSet _description;
        [SerializeField][Min(3f)] private float _constructionTime = 5f;

        public Sprite Icon => _icon;

        public ILanguageVisitor Name => _name;

        public ILanguageVisitor Description => _description;

        public float ConstructionTime => _constructionTime;

        public TeamType TeamType { get; } = TeamType.None;
    }
}
