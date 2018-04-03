using UnityEngine;
using System.Collections;
using AdvancedInspector;
using Scoring_Data_Block;
using System;
public class ScoreSysScript
{
    #region singletonInstance
    private static ScoreSysScript instance;
    public static ScoreSysScript Instance
    {
        get
        {
            if (instance == null)
                instance = new ScoreSysScript();
            return instance;
        }
    }
    #endregion

    #region init
    _ChangeScoresToBinaryData BinaryData;
    Scoring_Block scene_data = null;
    public void Init()
    {
        BinaryData = _ChangeScoresToBinaryData.Instance;
        scene_data = BinaryData.LoadBinaryData();
    }
    #endregion

    #region validworldlevelfloats
    [Restrict("ValidWorldInts"), Toolbar("DisplayWorldLevelToolBar", Style = "Toolbar", Priority = 0), Style("toolbarDropDown")]
    public int display_world;
    [Restrict("ValidLevelInts"), Toolbar("DisplayWorldLevelToolBar", Style = "Toolbar"), Style("toolbarDropDown")]
    public int display_level;

    private IList ValidWorldInts()
    {
        return new int[] { 1, 2, 3, 4 };
    }

    private IList ValidLevelInts()
    {
        return new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    }
    #endregion

    #region scoretoolbar
    [ReadOnly, Toolbar("DisplayImmutableScoreToolbar", Style = "Toolbar", Priority = 1), Style("Label")]
    public int Current_Gold;
    [ReadOnly, Toolbar("DisplayImmutableScoreToolbar", Style = "Toolbar"), Style("Label")]
    public int Current_Silver;
    [ReadOnly, Toolbar("DisplayImmutableScoreToolbar", Style = "Toolbar"), Style("Label")]
    public int Current_Bronze;

    [Toolbar("DisplayScoreToolbar", Style = "Toolbar", Priority = 2), Style("ToolbarTextField")]
    public int Modified_Gold;
    [Toolbar("DisplayScoreToolbar", Style = "Toolbar"), Style("ToolbarTextField")]
    public int Modified_Silver;
    [Toolbar("DisplayScoreToolbar", Style = "Toolbar"), Style("ToolbarTextField")]
    public int Modified_Bronze;
    #endregion

    block_data identified_block = null;

    [Inspect, Descriptor(Name = "Load the valid World/Level scores"),Group("Normal people use this")]
    void LoadWorld()
    {
        if (scene_data == null)
            Init();
        if (display_level == 0 || display_world == 0)
        {
            Debug.LogError("Please give a world/level to load!");
            return;
        }
        identified_block = FindBlockDataScene(scene_data, display_world, display_level);
        if (identified_block == null)
        {
            Debug.LogError("Cannot find corresponding world/level");
        }
        Current_Gold = identified_block.gold;
        Current_Silver = identified_block.silver;
        Current_Bronze = identified_block.bronze;
    }

    block_data FindBlockDataScene(Scoring_Block scene, int world, int level)
    {
        foreach (var data in scene.data)
        {
            if (data.world == world && data.level == level)
            {
                return data;
            }
        }
        return null;
    }

    [Inspect, Descriptor(Name = "Save the current scores in"), Group("Normal people use this", 1)]
    void Save()
    {
        if(identified_block == null)
        {
            Debug.LogError("You have to load before you save!");
            return;
        }

        identified_block.bronze = Modified_Bronze;
        identified_block.silver = Modified_Silver;
        identified_block.gold = Modified_Gold;


        if (BinaryData.SaveBinaryData(scene_data))
        {
            Debug.Log("Successfully saved");
            LoadWorld();
        }
        else
            Debug.Log("Something failed...");
    }

    [Spacing(Before = 50), Inspect, Group("Create new save block", 2, Description = "Only use this if you are ROLAND")]
    public block_data newdata = null;

    [Inspect, Group("Create new save block")]
    public void CreateNewBlock()
    {
        if (scene_data == null)
            Init();
        if(newdata == null)
        {
            Debug.LogError("New data is null. Are you Roland? Aborting.");
            return;
        }

        //We must first find if there is already a world/level inside the file
        identified_block = FindBlockDataScene(scene_data, display_world, display_level);
        if(identified_block != null)
        {
            Debug.LogError("I found a world/level that is the same as the block I'm saving! Aborting save!");
        }
        Array.Resize<block_data>(ref scene_data.data, scene_data.data.Length + 1);

        scene_data.data[scene_data.data.Length - 1] = newdata;

        if (BinaryData.SaveBinaryData(scene_data))
        {
            Debug.Log("Successfully saved");
        }
        else
            Debug.Log("Something failed...");

    }
}