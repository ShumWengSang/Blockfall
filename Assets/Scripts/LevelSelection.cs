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
        for (int k = 1; k < WorldBGs.Length + 1; k++)
        {
            for (int i = 1; i < WorldBGs[k - 1].childCount + 1; i++)
            {
                WorldButtons[k - 1, i - 1] = WorldBGs[k - 1].GetChild(i - 1);
                WorldButtons[k - 1, i - 1].GetComponentInChildren<Text>().text = (i).ToString();
                int Pref = PlayerPrefs.GetInt("Level" + k + "-" + i, -1);
                if (Pref >= 0)
                {
                    //Level finished.
                    //Give the appropriate medal.

                    switch (Pref)
                    {
                        case 1:
                            WorldButtons[k - 1, i - 1].GetChild(1).GetComponent<Image>().sprite = TrashTrophy;
                            break;
                        case 2:
                            WorldButtons[k - 1, i - 1].GetChild(1).GetComponent<Image>().sprite = BronzeTrophy;
                            break;
                        case 3:
                            WorldButtons[k - 1, i - 1].GetChild(1).GetComponent<Image>().sprite = SilverTrophy;
                            break;
                        case 4:
                            WorldButtons[k - 1, i - 1].GetChild(1).GetComponent<Image>().sprite = GoldTrophy;
                            break;
                    }

                    WorldButtons[k - 1, i - 1].GetComponent<Button>().interactable = true;
                }
                else
                {
                    WorldButtons[k - 1, i - 1].GetChild(1).GetComponent<Image>().sprite = NoTrophy;
                    if(FinishedLastLevel)
                    {
                        FinishedLastLevel = false;
                        WorldButtons[k - 1, i - 1].GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        WorldButtons[k - 1, i - 1].GetComponent<Button>().interactable = false;
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
        for(int i = 1; i <= FakeWorld; i++)
        {
            if(i != FakeWorld) //If not max fake world set all trophies
            {
                for (int j = 1; j <= 12; j++)
                {
                    PlayerPrefs.SetInt("Level" + i.ToString() + "-" + j.ToString(), TrophyNumber);
                }
            }
            for(int j = 1; j <= FakeLevel; j++) //else set to fakelevel
            {
                PlayerPrefs.SetInt("Level" + i.ToString() + "-" + j.ToString(), TrophyNumber);
            }
        }
    }

    void Start()
    {
        //FakePlayerPrefData();
        SetButtons();
    }
}
