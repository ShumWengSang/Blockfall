using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour {
    ScreenManager screenManager;
    public Animator screenTransAnim; 
    public float waitSeconds = 0.5f;
    WaitForSeconds wait;
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
        Debug.Log("Loading level");
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

    public void LoadNextLevel()
    {

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
