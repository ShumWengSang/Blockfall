using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

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
        string currentSceneString = EditorSceneManager.GetActiveScene().name;

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

            SceneManager.LoadScene("Level" + currentWorldInt.ToString() + "-1");
        }
        else
        {
            Debug.Log("Level" + currentWorldString + "-" + nextLevelInt.ToString());
            SceneManager.LoadScene("Level" + currentWorldString + "-" + nextLevelInt.ToString());
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
