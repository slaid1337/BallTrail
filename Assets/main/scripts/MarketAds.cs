using UnityEngine;
using Eiko.YaSDK;

public class MarketAds : MonoBehaviour
{
    private YandexSDK _yandexSDK;
    [SerializeField] private Money _money;

    private void Start()
    {
        _yandexSDK = YandexSDK.instance;
    }

    public void ShowRewardFor15()
    {
        _yandexSDK.ShowRewarded("15");
        _yandexSDK.onRewardedAdReward += Reward15;
        _yandexSDK.onRewardedAdError += DisableReward;
        _yandexSDK.onRewardedAdClosed += DisableReward;
    }

    public void DisableReward(int i)
    {
        _yandexSDK.onRewardedAdClosed -= DisableReward;
        _yandexSDK.onRewardedAdError -= DisableReward;
        _yandexSDK.onRewardedAdReward -= Reward15;
    }

    public void DisableReward(string str)
    {
        _yandexSDK.onRewardedAdClosed -= DisableReward;
        _yandexSDK.onRewardedAdError -= DisableReward;
        _yandexSDK.onRewardedAdReward -= Reward15;
    }

    public void Reward15(string str)
    {
        _money.money += 15;
        _yandexSDK.onRewardedAdReward -= Reward15;
    }
}
