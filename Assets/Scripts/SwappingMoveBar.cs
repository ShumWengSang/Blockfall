using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwappingMoveBar : MonoBehaviour {

    public RectTransform TrophyMarker;

    public Image ProgressBar; //Part that scales

    public Sprite GoldSprite;
    public Sprite SilverSprite;
    public Sprite BronzeSprite;
    public Sprite AluminiumSprite;

    public GameObject ScoreManager;

    public Text MovesMade;

    public float progressRate; //How fast the bar fills out when you make a move

    public RotateGrid rotateScript;

    float goldRank, silverRank, bronzeRank, barLength;

    Image currentTrophy;
    float currentTargetMoves;

    float lowestRank;
    float lowestBase;

    // Use this for initialization
    void Start () {
        ScoreSystem sys = ScoreManager.GetComponent<ScoreSystem>();
        goldRank = sys.GoldRank;
        silverRank = sys.SilverRank;
        bronzeRank = sys.BronzeRank;
        barLength = TrophyMarker.parent.GetComponent<RectTransform>().rect.width;

       // TrophyMarker.GetComponent<Image>(). = GoldImage;
        TrophyMarker.GetChild(0).GetComponent<Text>().text = " ≤ " + goldRank.ToString();

        progressRate = 0.005f;
        ProgressBar.fillAmount = 1;

        lowestRank = goldRank;
        lowestBase = goldRank;
    }
	
	// Update is called once per frame
	void Update () {
        MovesMade.text = rotateScript.TimesMoved.ToString();

        /*
        if (rotateScript.TimesMoved <= goldRank)
        {
            lowestRank = goldRank;
            lowestBase = goldRank;
        }
        else if (rotateScript.TimesMoved <= silverRank)
        {
            lowestRank = goldRank + silverRank;
            lowestBase = silverRank - goldRank;
        }
        else if (rotateScript.TimesMoved <= bronzeRank)
        {
            lowestRank = goldRank + silverRank + bronzeRank;
            lowestBase = bronzeRank - silverRank;
        }*/

        if (ProgressBar.fillAmount > ((lowestRank) - rotateScript.TimesMoved) / (lowestBase))
        {
            ProgressBar.fillAmount -= progressRate;
            if (ProgressBar.fillAmount <= 0)
            {
                ProgressBar.fillAmount = 1;
                if (rotateScript.TimesMoved < silverRank)
                {
                    TrophyMarker.GetComponent<Image>().sprite = SilverSprite;
                    TrophyMarker.GetChild(0).GetComponent<Text>().text = " ≤ " + silverRank.ToString();

                    lowestRank = rotateScript.TimesMoved - goldRank;
                    lowestBase = silverRank - goldRank;
                }
                else if (rotateScript.TimesMoved < bronzeRank)
                {
                    TrophyMarker.GetComponent<Image>().sprite = BronzeSprite;
                    TrophyMarker.GetChild(0).GetComponent<Text>().text = " ≤ " + bronzeRank.ToString();

                    lowestRank = rotateScript.TimesMoved - silverRank;
                    lowestBase = bronzeRank - silverRank;
                }
            }
        }
        /*
        if ( (rotateScript.TimesMoved == lowestRank + 1) && (ProgressBar.fillAmount != 1) )
        {
            ProgressBar.fillAmount -= progressRate;
            if (ProgressBar.fillAmount <= 0)
            {
                ProgressBar.fillAmount = 1;

                if (rotateScript.TimesMoved <= goldRank)
                {
                    lowestRank = goldRank;
                }
                else if (rotateScript.TimesMoved <= silverRank)
                {
                    lowestRank = goldRank + silverRank;
                    lowestBase = silverRank - goldRank;
                }
                else if (rotateScript.TimesMoved <= bronzeRank)
                {
                    lowestRank = goldRank + silverRank + bronzeRank;
                    lowestBase = bronzeRank - silverRank;
                }
            }   
        }
        else
        {
            if (ProgressBar.fillAmount > ((lowestRank + 1) - rotateScript.TimesMoved) / (lowestBase + 1))
            {
                ProgressBar.fillAmount -= progressRate;
            }
        }

        */

        /*
        if (rotateScript.TimesMoved <= goldRank)
        {
            lowestRank = goldRank;
        }
        else if (rotateScript.TimesMoved <= silverRank)
        {
            lowestRank = goldRank + silverRank;
        }
        else if (rotateScript.TimesMoved <= bronzeRank)
        {
            lowestRank = goldRank + silverRank + bronzeRank;
        }
        */

    }
}
