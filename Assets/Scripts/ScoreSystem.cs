using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AdvancedInspector;
using UnityEngine.SceneManagement;
public class ScoreSystem : MonoBehaviour
{
    public delegate void OnLevelParsed(int world, int level);
    public static event OnLevelParsed OnParsed;

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
    public Text EndMedalText;
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

    [Inspect]
    public void UpdateWorldLevelInts()
    {
        string currentSceneString = SceneManager.GetActiveScene().name;

        int posOfDash = currentSceneString.IndexOf("-");

        if (currentSceneString.Contains("Level"))
        {
            //Parse current world in string form
            int firstWorldDigit = currentSceneString.IndexOf("l") + 1;
            string currentWorldString = currentSceneString.Substring(firstWorldDigit, posOfDash - firstWorldDigit); //This is in case we have world in double digits. 
            int currentWorldInt = int.Parse(currentWorldString);

            //Parse current level in string then int form
            string currentLevelString = currentSceneString.Substring(posOfDash + 1);
            int currentLevelInt = int.Parse(currentLevelString);

            World = currentWorldInt;
            level = currentLevelInt;
            if (OnParsed != null) OnParsed(World, level);
        }
    }

    void Start()
    {
        UpdateWorldLevelInts();

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
        string endMedalText;
        var movedUsed = rotateScript.TimesMoved;
        int result = 1;
        if(movedUsed <= GoldRank)
        {
            result = 4;
            onGameCompleteMedel.sprite = GoldMedal;
            endMedalText = "Gold Rank!";
            Debug.Log("You get gold rank");
        }
        else if(movedUsed <= SilverRank)
        {
            result = 3;
            onGameCompleteMedel.sprite = SilverMedal;
            endMedalText = "Silver Rank!";
            Debug.Log("You got silver rank");
        }
        else if(movedUsed <= BronzeRank)
        {
            result = 2;
            onGameCompleteMedel.sprite = BronzeMedal;
            endMedalText = "Bronze Rank!";
            Debug.Log("You got bronze");
        }
        else
        {
            result = 1;
            onGameCompleteMedel.sprite = Aluminum;
            endMedalText = "Runner Up!";
            Debug.Log("You got aluminium");
        }
        EndMedalText.text = "Medal : " + endMedalText;
        NumberOfMoves.text = "Moves : " + movedUsed.ToString();
        gameOver.SetActive(true);
        FinishedPuzzle(result);
    }

    void FinishedPuzzle(int result)
    {
        Debug.Log("Level: " + World + "-" + level + " result is " + result);
        PlayerPrefs.SetInt("Level" + World + "-" + level, result);
    }
}
