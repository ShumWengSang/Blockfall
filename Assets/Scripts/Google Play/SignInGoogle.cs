using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class SignInGoogle : MonoBehaviour {
    private GameObject obj;

    void BuildGooglePlayConfig()
    {
        
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    // Use this for initialization
    void Start()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            BuildGooglePlayConfig();
            AuthenticatePlay();
        }
    }

    void AuthenticatePlay()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            //handle success or failure
            if (success)
            {
                ((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.BOTTOM);
                Debug.Log("Successful log in.");

                Social.ReportProgress("CgkItIPS7NIVEAIQAQ", 100.0f, (bool success_2) =>
                {
                    if (success_2)
                        Social.ShowAchievementsUI();
                    // handle success or failure
                });
            }
            else
            {
                Debug.Log("Authentication Fail");
            }
        });
    }
}
