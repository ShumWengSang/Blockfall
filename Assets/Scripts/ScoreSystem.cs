using UnityEngine;
using System.Collections;
using AdvancedInspector;
public class ScoreSystem : MonoBehaviour
{
    static ScoreSystem instance;
    [Inspect(InspectorLevel.Advanced)]
    public RotateGrid rotateScript;

    [Group("Ranking System"), Inspect]
    int GoldRank;
    [Group("Ranking System"), Inspect]
    int SilverRank;
    [Group("Ranking System"), Inspect]
    int BronzeRank;

    void Awake()
    {
        instance = this;
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
