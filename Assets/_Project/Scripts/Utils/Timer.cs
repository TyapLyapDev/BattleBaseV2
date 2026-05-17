using System;

namespace BattleBase.Utils
{
    public class Timer
    {
        private float _timeWait;
        private float _waitingTime = 0;

        public Timer(float timeWait)
        {
            if (timeWait <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeWait));

            _timeWait = timeWait;
        }

        public bool IsTimeUp => _waitingTime <= 0;

        public void SkipTick(float delta)
        {
            if (delta <= 0)
                throw new ArgumentOutOfRangeException(nameof(delta));

            if (IsTimeUp == false)
                _waitingTime -= delta;
        }

        public void RestartTimer()
        {
            _waitingTime = _timeWait;
        }

        public void SetWaitTime(float timeWait)
        {
            if (timeWait <= 0)
                throw new ArgumentOutOfRangeException(nameof(timeWait));

            _timeWait = timeWait;
        }
    }
}