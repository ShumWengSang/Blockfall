

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AdvancedInspector;
using UnityEngine.SceneManagement;
using Scoring_Data_Block;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreSystem : MonoBehaviour
{
    public bool isReal_testing = false;
    public delegate void OnLevelParsed(int world, int level);
    public static event OnLevelParsed OnParsed;

    //[Group("Game UI"), Inspect]
    public int World = 1;
    //[Group("Game UI"), Inspect]
    public int level = 1;

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
        GetPlayerPrefData();
        UpdateWorldLevelInts();
    }

    void GetPlayerPrefData()
    {
        int local_world = PlayerPrefs.GetInt("CurrentWorld", 0);
        int local_level = PlayerPrefs.GetInt("CurrentLevel", 0);

        //Scoring_Block data = BinarySerializor.DeserializeFromBinary<Scoring_Block>(Application.streamingAssetsPath + "/Data/ScoreSystem.sco");
        WWW file = new WWW(Application.streamingAssetsPath + "/Data/ScoreSystem.sco");
        Scoring_Block data;
        while (!file.isDone)
        {

        }
        if (file.error != null)
            return;
        MemoryStream mem = new MemoryStream(file.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        data = (Scoring_Block)bf.Deserialize(mem);

        mem.Close();
        mem.Dispose();
        foreach (var iter in data.data)
        {
            if(iter.world == local_world)
                if(iter.level == local_level)
                {
                    GoldRank = iter.gold;
                    SilverRank = iter.silver;
                    BronzeRank = iter.bronze;
                }
        }
#if OLDLOADDATA
        int local_world = PlayerPrefs.GetInt("CurrentWorld", 0);
        int local_level = PlayerPrefs.GetInt("CurrentLevel", 0);

        GoldRank = PlayerPrefs.GetInt("GoldRank_" + local_world + "_" + local_level, 4);
        SilverRank = PlayerPrefs.GetInt("SilverRank_" + local_world + "_" + local_level, 6);
        BronzeRank = PlayerPrefs.GetInt("BronzeRank_" + local_world + "_" + local_level, 8);
#else

#endif
    }

    [Inspect]
    public void SetPlayerPrefData()
    {
        //DEPRECATED
        return;
    }

    [Inspect]
    void UpdateTexts()
    {
        Ranks.text = "Gold: " + GoldRank.ToString() + "\nSilver: " + SilverRank.ToString() + "\nBronze: " + BronzeRank.ToString();
        Level.text = World.ToString() + " - " + level.ToString();
    }

    public void UpdateWorldValues_OLD()
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
        }
    }

    [Inspect]
    public void UpdateWorldLevelInts()
    {
        World = PlayerPrefs.GetInt("CurrentWorld",0);
        level = PlayerPrefs.GetInt("CurrentLevel",0);
        if (OnParsed != null) OnParsed(World, level);
       // if (isReal_testing)
        {
            //the above condition checks is a condition that we must manually change. Usually should be false, unless we are testing saveloadbridge
            //or from loading from another scene.
            //basically using saveloadbridge in realtime
//#if UNITY_EDITOR_WIN
            string path = Application.streamingAssetsPath + "/Data/" + World.ToString() + "-" + level.ToString() + ".dat";
            //#elif UNITY_ANDROID
            //string path = Application.streamingAssetsPath + "/Data/" + World.ToString() + "-" + level.ToString() + ".dat";
            //string path = "jar:file://" + Application.dataPath + "!/assets/" + "/Data/" + World.ToString() + "-" + level.ToString() + ".dat";
            //#endif
            if (!SaveLoadBridge.Instance.LoadScene(path))
                GetComponent<SceneChanger>().DirectlyMainMenu();
        }
    }

    void Start()
    {
        UpdateTexts();
        gameOver.SetActive(false);
    }

    void OnDestroy()
    {
    }
    
    void OnEnable()
    {
        GoalChecker.OnFinishedGame += OnFinishedGame;
    }

    void OnDisable()
    {
        GoalChecker.OnFinishedGame -= OnFinishedGame;

        if(!gameOver.activeSelf)
            FinishedPuzzle(0);
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
        }
        else if(movedUsed <= SilverRank)
        {
            result = 3;
            onGameCompleteMedel.sprite = SilverMedal;
            endMedalText = "Silver Rank!";
        }
        else if(movedUsed <= BronzeRank)
        {
            result = 2;
            onGameCompleteMedel.sprite = BronzeMedal;
            endMedalText = "Bronze Rank!";
        }
        else
        {
            result = 1;
            onGameCompleteMedel.sprite = Aluminum;
            endMedalText = "Runner Up!";
        }
        EndMedalText.text = "Medal : " + endMedalText;
        NumberOfMoves.text = "Moves : " + movedUsed.ToString();
        gameOver.SetActive(true);
        FinishedPuzzle(result);
    }

    void FinishedPuzzle(int result)
    {
        int currentResult = PlayerPrefs.GetInt("Level" + World + "-" + level);
        if(currentResult <= result)
            PlayerPrefs.SetInt("Level" + World + "-" + level, result);
    }
}
