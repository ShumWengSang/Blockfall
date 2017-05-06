using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdmobBanner_PauseScreen : MonoBehaviour {
    List<BannerView> banners = new List<BannerView>();

    void OnEnable()
    {
        if(banners.Count == 0)
            initAdmob();
        foreach (BannerView view in banners)
        {
            view.Show();
        }
    }

    void OnDisable()
    {
        foreach (BannerView view in banners)
        {
            view.Hide();
        }
    }

    void initAdmob()
    {
        RequestBanner("ca-app-pub-3458078707660764/4023647339", AdPosition.Top);
        RequestBanner("ca-app-pub-3458078707660764/2069929733", AdPosition.Bottom);
    }

    private void RequestBanner(string adUnitId, AdPosition pos)
    {
        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, pos);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);

        banners.Add(bannerView);
    }


}
