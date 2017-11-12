using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedInspector;
using Scoring_Data_Block;

public class _ChangeScoresToBinaryData : MonoBehaviour {
    List<block_data> AllPlayerPrefScores;

    [Inspect]
    public void ChangeAllScoreToBinary()
    {
        //First we want to load all the data into memory
        LoadAllPlayerPrefScore();

        //Now we put the data as binary
        Scoring_Block data = new Scoring_Block();
        data.data = AllPlayerPrefScores.ToArray();
        BinarySerializor.SerializeToBinary<Scoring_Block>(Application.streamingAssetsPath + "/Data/ScoreSystem.sco", data);
            //Debug.Log("Successfully saved scores");
    }

    block_data LoadPlayerPrefScore(int world, int level)
    {
        block_data newScore = new block_data();
        newScore.world = world;
        newScore.level = level;
        newScore.gold = PlayerPrefs.GetInt("GoldRank_" + world + "_" + level, 4);
        newScore.silver = PlayerPrefs.GetInt("SilverRank_" + world + "_" + level, 6);
        newScore.bronze = PlayerPrefs.GetInt("BronzeRank_" + world + "_" + level, 8);
        return newScore;
    }

    void LoadAllPlayerPrefScore()
    {
        AllPlayerPrefScores = new List<block_data>();
        for (int world = 0; world < 4; world++)
        {
            for(int level = 0; level < 12; level++)
            {
                AllPlayerPrefScores.Add(LoadPlayerPrefScore(world + 1, level + 1));
            }
        }
    }
}
