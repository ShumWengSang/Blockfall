using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class VideoRewardAd : MonoBehaviour {
    //ca-app-pub-3458078707660764/7616242139
    public RotateGrid grid;
    RewardBasedVideoAd rewardVideo;
    private void OnEnable()
    {
        grid.PauseFinger();
        if (rewardVideo == null)
            Init("ca-app-pub-3458078707660764/7616242139");
        RewardBasedVideoAd.Instance.OnAdRewarded += Instance_OnAdRewarded;
    }

    private void OnDisable()
    {
        RewardBasedVideoAd.Instance.OnAdRewarded -= Instance_OnAdRewarded;
    }

    private void Init(string videoID)
    {
        rewardVideo = RewardBasedVideoAd.Instance;
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).Build();
        rewardVideo.LoadAd(request, videoID);
    }

    private void Instance_OnAdRewarded(object sender, Reward e)
    {
        throw new System.NotImplementedException();
    }

    public void ShowVideo()
    {
        if (rewardVideo.IsLoaded())
        {
            Debug.Log("Video loaded, playing");
            rewardVideo.Show();
        }
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print("User rewarded with: " + amount.ToString() + " " + type);
    }
}
