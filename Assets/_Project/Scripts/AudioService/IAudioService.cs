namespace BattleBase.AudioService
{
    public interface IAudioService
    {
        IMusic Music { get; }

        ISfx Sfx { get; }
    }
}