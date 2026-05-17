namespace BattleBase.Gameplay.CameraNavigation
{
    public readonly struct CornerBounds
    {
        public readonly float MinX;
        public readonly float MaxX;
        public readonly float MinZ;
        public readonly float MaxZ;

        public CornerBounds(float minX, float maxX, float minZ, float maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinZ = minZ;
            MaxZ = maxZ;
        }
    }
}