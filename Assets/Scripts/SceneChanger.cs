using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
public class SceneChanger : MonoBehaviour {
    ScreenManager screenManager;
    public Animator screenTransAnim; 
    public float waitSeconds = 0.5f;
    WaitForSeconds wait;
    public GameObject WatchAdToContinue;
    public GameObject Background;
    static int InternalResetCounter = 0;
    void Awake()
    {
        wait = new WaitForSeconds(waitSeconds);
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
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                InternalResetCounter = 0;
                ChangeScene(SceneManager.GetActiveScene().name);
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void LoadNextLevel()
    {
        string currentSceneString = SceneManager.GetActiveScene().name;

        int posOfDash = currentSceneString.IndexOf("-");

        //Parse current world in string form
        int firstWorldDigit = currentSceneString.IndexOf("l") + 1;
        string currentWorldString = currentSceneString.Substring(firstWorldDigit, posOfDash - firstWorldDigit); //This is in case we have world in double digits. 
        int currentWorldInt;

        //Parse current level in string then int form
        string currentLevelString = currentSceneString.Substring(posOfDash + 1);
        int currentLevelInt = int.Parse(currentLevelString);
        int nextLevelInt = currentLevelInt += 1;


        if (nextLevelInt > 15)
        {
            currentWorldInt = int.Parse(currentWorldString);
            currentWorldInt += 1;

            ChangeScene("Level" + currentWorldInt.ToString() + "-1");
        }
        else
        {
            Debug.Log("Level" + currentWorldString + "-" + nextLevelInt.ToString());
            ChangeScene("Level" + currentWorldString + "-" + nextLevelInt.ToString());
        }
    }

    IEnumerator loadNextScene(string scene)
    {
        while(screenTransAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !screenTransAnim.IsInTransition(0))
        {
            yield return null;
        }
        yield return SceneManager.LoadSceneAsync(scene);
    }
}
