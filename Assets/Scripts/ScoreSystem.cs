using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AdvancedInspector;
public class ScoreSystem : MonoBehaviour
{
    [Group("Game UI"), Inspect]
    public int World = 1;
    [Group("Game UI"), Inspect]
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
        if(movedUsed <= GoldRank)
        {
            Debug.Log("You get gold rank");
        }
        else if(movedUsed <= SilverRank)
        {
            Debug.Log("You got silver rank");
        }
        else if(movedUsed <= BronzeRank)
        {
            Debug.Log("You got bronze");
        }
        else
        {
            Debug.Log("You got aluminium");
        }
        NumberOfMoves.text = "Moves : " + movedUsed.ToString();
        gameOver.SetActive(true);
    }
}
