using System;
using UnityEngine;

namespace BattleBase.Utils
{
    public static class CameraDragHelper
    {
        private const float OrthographicSizeMultiplier = 2f;

        public static Vector3 ConvertPixelDeltaToWorldDelta(Camera camera, Vector2 pixelDelta)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));

            float worldHeight = camera.orthographicSize * OrthographicSizeMultiplier;
            float worldWidth = worldHeight * camera.aspect;
            float worldDeltaX = pixelDelta.x / Screen.width * worldWidth;
            float worldDeltaZ = pixelDelta.y / Screen.height * worldHeight;

            return new Vector3(worldDeltaX, 0f, worldDeltaZ);
        }
    }
}