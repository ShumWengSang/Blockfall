using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Advertisements;
public class SceneChanger : MonoBehaviour {
    ScreenManager screenManager;
    public Animator LoadingWords;
    public Animator screenTransAnim; 
    public float waitSeconds = 0.5f;
    public GameObject WatchAdToContinue;
    public GameObject Background;
    static int InternalResetCounter = 0;
    public int AmountToShowVideo = 10;
    void Awake()
    {
        screenManager = GetComponent<ScreenManager>();
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelWasLoaded_new;
        ScoreSystem.OnBackButtonDowned += OnEscapeButtonDown;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelWasLoaded_new;
        ScoreSystem.OnBackButtonDowned -= OnEscapeButtonDown;
    }

    void Start()
    {
        screenManager.OpenPanel(screenTransAnim);

        if(WatchAdToContinue != null)
        {
            WatchAdToContinue.GetComponent<InterstitialAd_Show>().Init();
        }
    }
    void OnLevelWasLoaded_new(Scene scene, LoadSceneMode mode)
    {
        screenManager.OpenPanel(screenTransAnim);
    }
    public void DirectlyMainMenu()
    {
        SceneManager.LoadSceneAsync("LevelSelection");
    }

    public void ChangeScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        LoadingWords.SetTrigger("Loading");
        screenTransAnim.transform.SetAsLastSibling();
        screenManager.CloseCurrent(scene);
        //StartCoroutine(loadNextScene(scene, mode));
        SceneManager.LoadSceneAsync(scene);
    }

    public void LoadMainMenu()
    {
        ChangeScene("LevelSelection");
    }

    public void ReloadScene()
    {
        InternalResetCounter++;
        if(InternalResetCounter >= AmountToShowVideo)
        {
            InternalResetCounter = 0;
            Background.SetActive(true);
            WatchAdToContinue.SetActive(true);
        }
        else
        {
            ChangeScene(SceneManager.GetActiveScene().name);
        }
    }

    public void DirectReloadScene()
    {
        ChangeScene(StaticString.MasterGameScene);
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
   public void EditorDirectLoadNextLevel()
    {
        int World = PlayerPrefs.GetInt(StaticString.CurrentWorld, 0);
        int level = PlayerPrefs.GetInt(StaticString.CurrentLevel, 0);
        int nextLevelInt = level + 1;
        int currentWorldInt = World;

        if (nextLevelInt < 13)
        {
            //not a new world
            PlayerPrefs.SetInt(StaticString.CurrentLevel, nextLevelInt);
        }
        else
        {
            PlayerPrefs.SetInt(StaticString.CurrentWorld, currentWorldInt + 1);
            PlayerPrefs.SetInt(StaticString.CurrentLevel, 1);
        }
        //UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MasterGameScene.unity");

    }

    public void LoadNextLevel()
    {
        int World = PlayerPrefs.GetInt(StaticString.CurrentWorld, 0);
        int level = PlayerPrefs.GetInt(StaticString.CurrentLevel, 0);
        int nextLevelInt = level + 1;
        int currentWorldInt = World;

        if (nextLevelInt < 13)
        {
            //not a new world
            PlayerPrefs.SetInt(StaticString.CurrentLevel, nextLevelInt);
        }
        else
        {
            PlayerPrefs.SetInt(StaticString.CurrentWorld, currentWorldInt + 1);
            PlayerPrefs.SetInt(StaticString.CurrentLevel, 1);
        }
        ChangeScene(StaticString.MasterGameScene);
    }

    IEnumerator loadNextScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        while(screenTransAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !screenTransAnim.IsInTransition(0))
        {
            yield return null;
        }
        yield return SceneManager.LoadSceneAsync(scene, mode);
    }

    public void Load_Scene_Directed(int world, int level)
    {
        PlayerPrefs.SetInt(StaticString.CurrentWorld, world);
        PlayerPrefs.SetInt(StaticString.CurrentLevel, level);

        ChangeScene(StaticString.MasterGameScene, LoadSceneMode.Additive);

       // string scene = StaticString.MasterGameScene;
        //LoadingWords.SetTrigger("Loading");
        //screenTransAnim.transform.SetAsLastSibling();
        //screenManager.CloseCurrent(scene);
        //StartCoroutine(Load_Scene_Direct_From_Menu_Coroutine(scene, LoadSceneMode.Additive));
    }

    IEnumerator Load_Scene_Direct_From_Menu_Coroutine(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        yield return SceneManager.LoadSceneAsync(scene, mode);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(StaticString.MasterGameScene));
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void OnEscapeButtonDown()
    {
        Debug.Log("Escape butrton scene ahcanger"); 
        LoadMainMenu();
    }
}
