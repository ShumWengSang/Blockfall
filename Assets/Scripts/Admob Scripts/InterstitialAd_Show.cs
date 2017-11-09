using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialAd_Show : MonoBehaviour {

    private InterstitialAd interstitial;

    private RewardBasedVideoAd rewardVideo;

    public SceneChanger LevelChanger;

    private void Awake()
    {

        Init();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    public void ShowInterAd()
    {
        if (interstitial.IsLoaded())
            interstitial.Show();
        else
            print("Ad not ready");
    }

    public void ShowRewardAd()
    {
        if (rewardVideo.IsLoaded())
            rewardVideo.Show();
        else
            print("reward not ready");
    }

    private void Init()
    {
//        bool DataNetwork;
//        string adUnityId = "unused";

//#if UNITY_ANDROID
//        switch (Application.internetReachability)
//        {
//            case NetworkReachability.ReachableViaCarrierDataNetwork:
//                //Interstitial ad
//                DataNetwork = true;
//                adUnityId = 
//                break;

//            case NetworkReachability.ReachableViaLocalAreaNetwork:
//                //Video Ad
//                DataNetwork = false;
//                break;

//            default:
//                break;
//        }

//#endif
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID

        //interstitial
        string adUnitId = "ca-app-pub-3458078707660764/7214982535";

        //reward video      
        adUnitId = "ca-app-pub-3458078707660764/7616242139";
#endif

        // Create an interstitial.
        this.interstitial = new InterstitialAd(adUnitId);
        this.rewardVideo = RewardBasedVideoAd.Instance;

        // Register for ad events.
        this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
        this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

        this.rewardVideo.OnAdLoaded += this.HandleRewardedLoaded;
        this.rewardVideo.OnAdRewarded += this.HandleRewardedReward;
        this.rewardVideo.OnAdFailedToLoad += this.HandleRewardedFailedToLoad;

        // Load an interstitial ad.
        this.interstitial.LoadAd(this.CreateAdRequest());

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardVideo.LoadAd(request, "ca-app-pub-3458078707660764/7616242139");
    }


#region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print(
           // "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialOpened event received");
        //close the ad window.
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialClosed event received");
        LevelChanger.ReloadScene();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

#region RewardedVideo Callback Handlers

    public void HandleRewardedLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardedFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardedReward(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialOpened event received");
        //close the ad window.
        LevelChanger.ReloadScene();
    }

    public void HandleRewardedClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleInterstitialClosed event received");
    }
    #endregion
}
