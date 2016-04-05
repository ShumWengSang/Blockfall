using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AdvancedInspector;
public class ScoreSystem : MonoBehaviour
{
    public int World = 1;
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

    public Text Level;
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
    }

    void OnDestroy()
    {
        instance = null;
    }
    
    void OnGameOver()
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
    }
}
