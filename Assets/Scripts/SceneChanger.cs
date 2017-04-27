using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements;
public class SceneChanger : MonoBehaviour {
    ScreenManager screenManager;
    public Animator screenTransAnim; 
    public float waitSeconds = 0.5f;
    public GameObject WatchAdToContinue;
    public GameObject Background;
    static int InternalResetCounter = 0;
    void Awake()
    {
        screenManager = GetComponent<ScreenManager>();
    }

    void Start()
    {
        screenManager.OpenPanel(screenTransAnim);
    }
    void OnLevelWasLoaded(int index)
    {
        screenManager.OpenPanel(screenTransAnim);
    }

    public void ChangeScene(string scene)
    {
        screenTransAnim.transform.SetAsLastSibling();
        screenManager.CloseCurrent(scene);
        StartCoroutine(loadNextScene(scene));
    }

    public void LoadMainMenu()
    {
        ChangeScene("LevelSelection");
    }

    public void ReloadScene()
    {
        InternalResetCounter++;
        if(InternalResetCounter > 5)
        {
            Background.SetActive(true);
            WatchAdToContinue.SetActive(true);
        }
        else
        {
            ChangeScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowRewardedAd()
    {
        //if (Advertisement.IsReady("rewardedVideo"))
        //{
        //    var options = new ShowOptions { resultCallback = HandleShowResult };
        //    Advertisement.Show("rewardedVideo", options);
        //}
    }
   // private void HandleShowResult(ShowResult result)
    //{
        //switch (result)
        //{
        //    case ShowResult.Finished:
        //        Debug.Log("The ad was successfully shown.");
        //        InternalResetCounter = 0;
        //        ChangeScene(SceneManager.GetActiveScene().name);
        //        //
        //        // YOUR CODE TO REWARD THE GAMER
        //        // Give coins etc.
        //        break;
        //    case ShowResult.Skipped:
        //        Debug.Log("The ad was skipped before reaching the end.");
        //        break;
        //    case ShowResult.Failed:
        //        Debug.LogError("The ad failed to be shown.");
        //        break;
        //}
   // }

    public void LoadNextLevel()
    {
        int World = PlayerPrefs.GetInt("CurrentWorld", 0);
        int level = PlayerPrefs.GetInt("CurrentLevel", 0);
        int nextLevelInt = level + 1;
        int currentWorldInt = World;

        if (nextLevelInt < 12)
        {
            //not a new world
            PlayerPrefs.SetInt("CurrentLevel", nextLevelInt);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentWorld", currentWorldInt + 1);
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }
        ChangeScene("MasterGameScene");
    }

    IEnumerator loadNextScene(string scene)
    {
        while(screenTransAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !screenTransAnim.IsInTransition(0))
        {
            yield return null;
        }
        yield return SceneManager.LoadSceneAsync(scene);
    }

    public void Load_Scene_Directed(int world, int level)
    {
        PlayerPrefs.SetInt("CurrentWorld", world);
        PlayerPrefs.SetInt("CurrentLevel", level);

        ChangeScene("MasterGameScene");
    }
}
