using System;

namespace BattleBase.AdvService
{
    public interface IAdvService
    {
        public event Action<string> RewardAdvShown;

        public void ShowInterstitialAdv();

        public void ShowRewardedAdv(string id);

        public void ShowRewardedAdv(string id, Action callback);

        public void SetActivityStickyAd(bool isActive);
    }
}