using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
public class VideoRewardAd : MonoBehaviour {
    //ca-app-pub-3458078707660764/7616242139
    public RotateGrid grid;
    RewardBasedVideoAd rewardVideo;
    private InterstitialAd interstitial;
    public SceneChanger scene;

    private void OnEnable()
    {
        grid.PauseFinger();
    }
    private void Awake()
    {
        Init("ca-app-pub-3458078707660764/7214982535");
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void Init(string videoID)
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = videoID;
#endif
        rewardVideo = RewardBasedVideoAd.Instance;
        rewardVideo.LoadAd(CreateAdRequest(), adUnitId);
        this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
    }

    public void ShowVideo()
    {
        if (interstitial.IsLoaded())
            interstitial.Show();
        else
            print("Ad not ready");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        scene.ReloadScene();
    }
}
