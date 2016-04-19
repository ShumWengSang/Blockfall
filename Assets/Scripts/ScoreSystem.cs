using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AdvancedInspector;
using UnityEngine.SceneManagement;
public class ScoreSystem : MonoBehaviour
{
    //[Group("Game UI"), Inspect]
    public int World = 1;
    //[Group("Game UI"), Inspect]
    public int level = 1;
    static ScoreSystem instance;
    [Inspect(InspectorLevel.Advanced)]
    public RotateGrid rotateScript;

    [Group("Ranking System"), Inspect]
    public int GoldRank;
    [Group("Ranking System"), Inspect]
    public int SilverRank;
    [Group("Ranking System"), Inspect]
    public int BronzeRank;

    [Group("On Game Complete"), Inspect]
    public GameObject gameOver;
    [Group("On Game Complete"), Inspect]
    public Image onGameCompleteMedel;
    [Group("On Game Complete"), Inspect]
    public Sprite GoldMedal;
    [Group("On Game Complete"), Inspect]
    public Sprite SilverMedal;
    [Group("On Game Complete"), Inspect]
    public Sprite BronzeMedal;
    [Group("On Game Complete"), Inspect]
    public Sprite Aluminum;
    [Group("On Game Complete"), Inspect]
    public Text NumberOfMoves;

    [Group("Game UI"), Inspect]
    public Text Level;
    [Group("Game UI"), Inspect]
    public Text Ranks;
    void Awake()
    {
        instance = this;
    }

    [Inspect]
    void UpdateTexts()
    {
        Ranks.text = "Gold: " + GoldRank.ToString() + "\nSilver: " + SilverRank.ToString() + "\nBronze: " + BronzeRank.ToString();
        Level.text = World.ToString() + " - " + level.ToString();
    }
    void Start()
    {
        string currentSceneString = SceneManager.GetActiveScene().name;

        int posOfDash = currentSceneString.IndexOf("-");

        //Parse current world in string form
        int firstWorldDigit = currentSceneString.IndexOf("l") + 1;
        string currentWorldString = currentSceneString.Substring(firstWorldDigit, posOfDash - firstWorldDigit); //This is in case we have world in double digits. 
        int currentWorldInt = int.Parse(currentWorldString);

        //Parse current level in string then int form
        string currentLevelString = currentSceneString.Substring(posOfDash + 1);
        int currentLevelInt = int.Parse(currentLevelString);

        World = currentWorldInt;
        level = currentLevelInt;

        UpdateTexts();
        gameOver.SetActive(false);
    }

    void OnDestroy()
    {
        instance = null;
    }
    
    void OnEnable()
    {
        GoalChecker.OnFinishedGame += OnFinishedGame;
    }

    void OnDisable()
    {
        GoalChecker.OnFinishedGame -= OnFinishedGame;
    }

    void OnFinishedGame()
    {
        var movedUsed = rotateScript.TimesMoved;
        int result = 1;
        if(movedUsed <= GoldRank)
        {
            result = 4;
            Debug.Log("You get gold rank");
        }
        else if(movedUsed <= SilverRank)
        {
            result = 3;
            Debug.Log("You got silver rank");
        }
        else if(movedUsed <= BronzeRank)
        {
            result = 2;
            Debug.Log("You got bronze");
        }
        else
        {
            result = 1;
            Debug.Log("You got aluminium");
        }
        NumberOfMoves.text = "Moves : " + movedUsed.ToString();
        gameOver.SetActive(true);
        FinishedPuzzle(result);
    }

    void FinishedPuzzle(int result)
    {
        PlayerPrefs.SetInt("Level" + World + "-" + level, result);
    }
}
