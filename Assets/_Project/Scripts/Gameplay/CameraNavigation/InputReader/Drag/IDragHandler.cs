using UnityEngine;

namespace BattleBase.Gameplay.CameraNavigation.InputReader
{
    public interface IDragHandler
    {
        Vector3? Update(float deltaTime);
    }
}