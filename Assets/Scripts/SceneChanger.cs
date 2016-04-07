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
        Debug.Log("Running this");
        screenManager.OpenPanel(screenTransAnim);
    }
    void OnLevelWasLoaded(int index)
    {
        Debug.Log("Loading level");
        screenManager.OpenPanel(screenTransAnim);
    }

    public void ChangeScene(string scene)
    {
        screenManager.CloseCurrent(scene);
        StartCoroutine(loadNextScene(scene));
    }

    public void LoadNextLevel()
    {

    }

    IEnumerator loadNextScene(string scene)
    {
        yield return wait;
        SceneManager.LoadSceneAsync(scene);
    }
}
