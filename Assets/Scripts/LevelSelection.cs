using UnityEngine;
using System.Collections;
using AdvancedInspector;
using UnityEngine.UI;
public class LevelSelection : MonoBehaviour
{
    [Group("Sprites")]
    public Sprite GoldTrophy;
    [Group("Sprites")]
    public Sprite SilverTrophy;
    [Group("Sprites")]
    public Sprite BronzeTrophy;
    [Group("Sprites")]
    public Sprite TrashTrophy;
    [Group("Sprites")]
    public Sprite NoTrophy;

    public Transform[] WorldBGs;

    [Inspect(InspectorLevel.Debug)]
    public Transform[,] WorldButtons;
    // Use this for initialization
    [Inspect]
    public void SetButtons()
    {
        bool FinishedLastLevel = true;
        WorldButtons = new Transform[WorldBGs.Length, WorldBGs[0].childCount];
        for (int k = 0; k < WorldBGs.Length; k++)
        {
            for (int i = 0; i < WorldBGs[k].childCount; i++)
            {
                WorldButtons[k, i] = WorldBGs[k].GetChild(i);
                WorldButtons[k, i].GetComponentInChildren<Text>().text = (i + 1).ToString();
                int Pref = PlayerPrefs.GetInt("Level" + k + "-" + i);
                Debug.Log("Working on " + k + " : " + i + " Prefs is : " + Pref);
                if (Pref >= 1)
                {
                    //Level finished.
                    //Give the appropriate medal.

                    switch (Pref)
                    {
                        case 1:
                            WorldButtons[k, i].GetChild(1).GetComponent<Image>().sprite = TrashTrophy;
                            break;
                        case 2:
                            WorldButtons[k, i].GetChild(1).GetComponent<Image>().sprite = BronzeTrophy;
                            break;
                        case 3:
                            WorldButtons[k, i].GetChild(1).GetComponent<Image>().sprite = SilverTrophy;
                            break;
                        case 4:
                            WorldButtons[k, i].GetChild(1).GetComponent<Image>().sprite = GoldTrophy;
                            break;
                    }

                    WorldButtons[k, i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    WorldButtons[k, i].GetChild(1).GetComponent<Image>().sprite = NoTrophy;
                    if(FinishedLastLevel)
                    {
                        FinishedLastLevel = false;
                        WorldButtons[k, i].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        WorldButtons[k, i].GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
    }
    [Group("Fake Player Pref Data")]
    public int FakeWorld;
    [Group("Fake Player Pref Data")]
    public int FakeLevel;
    [Group("Fake Player Pref Data")]
    public int TrophyNumber = 0;

    [Group("Fake Player Pref Data"), Inspect]
    void FakePlayerPrefData()
    {
        PlayerPrefs.DeleteAll();
        for(int i = 0; i < FakeWorld; i++)
        {
            if(i != FakeWorld - 1)
            {
                for (int j = 0; j < 15; j++)
                {
                    PlayerPrefs.SetInt("Level" + i.ToString() + "-" + j.ToString(), TrophyNumber);
                }
            }
            for(int j = 0; j < FakeLevel; j++)
            {
                PlayerPrefs.SetInt("Level" + i.ToString() + "-" + j.ToString(), TrophyNumber);
            }
        }
    }

    void Start()
    {
        SetButtons();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
