using System;
using YG;

namespace BattleBase.AdvService
{
    public class YandexGameAdvertisingAdapter : IAdvertisingService, IDisposable
    {
        public YandexGameAdvertisingAdapter()
        {
            YG2.onRewardAdv += OnReward;
        }

        public event Action<string> RewardAdvShown;

        public void Dispose() =>
            YG2.onRewardAdv -= OnReward;

        public void ShowInterstitialAdv() =>
            YG2.InterstitialAdvShow();

        public void ShowRewardedAdv(string id) =>
            YG2.RewardedAdvShow(id);

        public void ShowRewardedAdv(string id, Action callback) =>
            YG2.RewardedAdvShow(id, callback);

        public void SetActivityStickyAd(bool isActive) =>
            YG2.StickyAdActivity(isActive);

        private void OnReward(string id) =>
            RewardAdvShown?.Invoke(id);
    }
}