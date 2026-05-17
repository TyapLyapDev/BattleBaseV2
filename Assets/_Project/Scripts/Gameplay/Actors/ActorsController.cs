using System.Collections.Generic;
using UnityEngine;

namespace BattleBase.Gameplay.Actors
{
    public class ActorsController : MonoBehaviour, IActorsController
    {
        private List<IActor> _activeActors;

        private void Awake()
        {
            _activeActors = new List<IActor>();
        }

        public void AddActor(IActor actor)
        {
            if (actor == null)
                throw new System.ArgumentNullException(nameof(actor));

            _activeActors.Add(actor);
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < _activeActors.Count; i++)
            {
                IActor actor = _activeActors[i];

                if (actor.IsEnabled)
                    actor.Update(Time.fixedDeltaTime);
                else
                    _activeActors.RemoveAt(i--);
            }
        }
    }
}
