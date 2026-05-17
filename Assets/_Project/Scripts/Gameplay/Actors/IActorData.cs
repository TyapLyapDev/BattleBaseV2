using BattleBase.Localization;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    public interface IActorData : ITeamable
    {
        public Sprite Icon { get; }

        public ILanguageVisitor Name { get; }

        public ILanguageVisitor Description { get; }

        public float ConstructionTime { get; }
    }
}
