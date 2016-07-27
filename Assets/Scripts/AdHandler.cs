using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class AdHandler : MonoBehaviour {
    public string gameID = "1074800";
    BannerView bannerView;
	// Use this for initialization
	void Start () {
        RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3458078707660764/2069929733";
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
        Debug.Log("Banner loading");
    }



    public void ShowBanner()
    {
        bannerView.Show(); 
    }

    void Destroy()
    {
        bannerView.Destroy();
    }

}
