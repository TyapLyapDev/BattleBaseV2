using BattleBase.Localization;
using System;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    public class ActorDataModifier : IActorData
    {
        private readonly IActorData _data;
        private readonly float _constructionTimeCoefficient;

        public ActorDataModifier(IActorData data, TeamType teamType, float constructionTimeCoefficient = 1f)
        {
            if (constructionTimeCoefficient <= 0)
                throw new ArgumentOutOfRangeException(nameof(constructionTimeCoefficient));

            _data = data ?? throw new ArgumentNullException(nameof(data));
            _constructionTimeCoefficient = constructionTimeCoefficient;

            TeamType = teamType;
        }

        public Sprite Icon => _data.Icon;

        public ILanguageVisitor Name => _data.Name;

        public ILanguageVisitor Description => _data.Description;

        public float ConstructionTime => _data.ConstructionTime * _constructionTimeCoefficient;

        public TeamType TeamType { get; }
    }
}
