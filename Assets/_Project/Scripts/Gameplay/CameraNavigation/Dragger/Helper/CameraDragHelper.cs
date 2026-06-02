using System;
using UnityEngine;

namespace BattleBase.Utils
{
    public static class CameraDragHelper
    {
        private const float OrthographicSizeMultiplier = 2f;
        private const float GroundPlaneY = 0f;

        public static Vector3 ConvertPixelDeltaToWorldDelta(Camera camera, Vector2 pixelDelta)
        {
            if (camera.orthographic)
            {
                float worldHeight = camera.orthographicSize * OrthographicSizeMultiplier;
                float worldWidth = worldHeight * camera.aspect;
                float worldDeltaX = pixelDelta.x / Screen.width * worldWidth;
                float worldDeltaZ = pixelDelta.y / Screen.height * worldHeight;
                return new Vector3(worldDeltaX, 0f, worldDeltaZ);
            }
            else
            {
                Vector3 cameraPos = camera.transform.position;
                float distanceToGround = Mathf.Abs(cameraPos.y - GroundPlaneY);
                float vFOVrad = camera.fieldOfView * Mathf.Deg2Rad;
                float worldHeightAtGround = 2f * distanceToGround * Mathf.Tan(vFOVrad * 0.5f);
                float worldWidthAtGround = worldHeightAtGround * camera.aspect;

                float worldDeltaX = pixelDelta.x / Screen.width * worldWidthAtGround;
                float worldDeltaZ = pixelDelta.y / Screen.height * worldHeightAtGround;

                return new Vector3(worldDeltaX, 0f, worldDeltaZ);
            }
        }
    }
}