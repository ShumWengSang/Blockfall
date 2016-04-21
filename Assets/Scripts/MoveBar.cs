using UnityEngine.UI;
using UnityEngine;
using AdvancedInspector;
using System.Collections;

public class MoveBar : MonoBehaviour {
    /*
    public Image GoldMarker;
    public Image SilverMarker;
    public Image BronzeMarker;
    */

    [Group("Trophy Icons"), Inspect]
    public RectTransform GoldMarker;
    [Group("Trophy Icons"), Inspect]
    public RectTransform SilverMarker;
    [Group("Trophy Icons"), Inspect]
    public RectTransform BronzeMarker;

    public Image ProgressBar; //Part that scales

    private Image GoldImage;
    private Image SilverImage;
    private Image BronzeImage;

    public GameObject ScoreManager;

    public Text MovesMade;

    public float progressRate; //How fast the bar fills out when you make a move

    [Inspect(InspectorLevel.Advanced)]
    public RotateGrid rotateScript;

    float goldRank, silverRank, bronzeRank, barLength;

    // Use this for initialization
    void Start () {

        GoldImage = GoldMarker.GetComponent<Image>();
        SilverImage = SilverMarker.GetComponent<Image>();
        BronzeImage = BronzeMarker.GetComponent<Image>();

        ScoreSystem sys = ScoreManager.GetComponent<ScoreSystem>();
        goldRank = sys.GoldRank;
        silverRank = sys.SilverRank;
        bronzeRank = sys.BronzeRank;
        barLength = GoldMarker.parent.GetComponent<RectTransform>().rect.width;

        Vector3 goldPos = GoldMarker.localPosition;
        goldPos.x = goldRank / bronzeRank * barLength;
        GoldMarker.anchoredPosition = goldPos;

        Vector3 silPos = SilverMarker.localPosition;
        silPos.x = silverRank / bronzeRank * barLength;
        SilverMarker.anchoredPosition = silPos;

        GoldMarker.GetChild(1).GetComponent<Text>().text = goldRank.ToString();
        SilverMarker.GetChild(1).GetComponent<Text>().text = silverRank.ToString();
        BronzeMarker.GetChild(1).GetComponent<Text>().text = bronzeRank.ToString();

        progressRate = 0.005f;
        ProgressBar.fillAmount = 0;
    }

	// Update is called once per frame
	void Update () {
        if (ProgressBar.fillAmount < (float)rotateScript.TimesMoved / bronzeRank)
        {
            ProgressBar.fillAmount += progressRate;
        }

        MovesMade.text = rotateScript.TimesMoved.ToString();

        if (rotateScript.TimesMoved <= goldRank)
        {
            GoldImage.color = Color.white;
            SilverImage.color = Color.black;
            BronzeImage.color = Color.black;
        }
        else if (rotateScript.TimesMoved <= silverRank)
        {
            GoldImage.color = Color.black;
            SilverImage.color = Color.white;
            BronzeImage.color = Color.black;
        }
        else if (rotateScript.TimesMoved <= bronzeRank)
        {
            GoldImage.color = Color.black;
            SilverImage.color = Color.black;
            BronzeImage.color = Color.white;
        }
        else
        {
            GoldImage.color = Color.black;
            SilverImage.color = Color.black;
            BronzeImage.color = Color.black;
        }
    }
}
