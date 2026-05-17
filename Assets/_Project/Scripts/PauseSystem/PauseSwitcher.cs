using UnityEngine;

namespace BattleBase.PauseService
{
    public class PauseSwitcher : IPauseSwitcher
    {
        public void Pause() =>
            Time.timeScale = 0;

        public void Resume() =>
            Time.timeScale = 1;
    }
}