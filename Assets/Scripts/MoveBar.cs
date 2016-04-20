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

    public GameObject ScoreManager;

    [Inspect(InspectorLevel.Advanced)]
    public RotateGrid rotateScript;

    float goldRank, silverRank, bronzeRank, barLength;

    // Use this for initialization
    void Start () {
        goldRank = ScoreManager.GetComponent<ScoreSystem>().GoldRank;
        silverRank = ScoreManager.GetComponent<ScoreSystem>().SilverRank;
        bronzeRank = ScoreManager.GetComponent<ScoreSystem>().BronzeRank;
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
    }
	
	// Update is called once per frame
	void Update () {
        ProgressBar.GetComponent<Image>().fillAmount = (float)rotateScript.GetComponent<RotateGrid>().TimesMoved / bronzeRank;
	}
}
