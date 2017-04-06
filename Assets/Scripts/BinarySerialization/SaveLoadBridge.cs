using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data_Blocks;
using AdvancedInspector;
using UnityEditor;
using UnityEditor.SceneManagement;

//WE DON'T CARE TOO MUCH ABOUT OPTIMIZATION IN THIS SCRIPT
//EXCEPT FOR LOAD BECAUSE ITS NOT MEANT TO RUN AT RUNTIME

public class SaveLoadBridge {

    private static SaveLoadBridge instance;
    public static SaveLoadBridge Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveLoadBridge();
            return instance;
        }
    }
    Scene_Block scene = null;

    GameObject goal_block;
    GameObject iron_block;
    GameObject oneway_block_b;
    GameObject oneway_block_g;
    GameObject portal_block;
    GameObject vortex_block;
    GameObject wall_block;
    GameObject wood_block;

    public void Init()
    {
        string resourcePath = "Prefabs/Block Prefabs/";
        goal_block = Resources.Load(resourcePath + "Goal") as GameObject;
        iron_block = Resources.Load(resourcePath + "Iron Block") as GameObject;
        oneway_block_b = Resources.Load(resourcePath + "One Way Block") as GameObject;
        oneway_block_g = Resources.Load(resourcePath + "One Way Gate") as GameObject;
        portal_block = Resources.Load(resourcePath + "Portal") as GameObject;
        vortex_block = Resources.Load(resourcePath + "Vortex") as GameObject;
        wall_block = Resources.Load(resourcePath + "Wall Block") as GameObject;
        wood_block = Resources.Load(resourcePath + "Wood Block") as GameObject;
    }
    [ReadOnly]
    public int Current_World, Current_Level;
    [Space(2f)]
    public int new_world, new_level;


    [TextField(TextFieldType.File, "Load File", "Assets/Data")]
    public string LoadPath;

    [TextField(TextFieldType.Folder, "Save Parent File", "Assets/Data")]
    public string SaveParentFolder;

    [Inspect, Descriptor("Load d_ world and level", "Load the world - level as provided in D_. Only load a scene when using MasterGameScene")]
    public void LoadDebugScene()
    {
        Init();
        LoadScene(LoadPath);
    }

    [Inspect, Descriptor("Create new puzzle","Cretes a new puzzle by clearing the MasterGameScene. World and leve values are unimportant")]
    public void ClearSceneBlocks()
    {
        //find all the blocks and delete their gameobject
        Transform[] allobj = GameObject.FindObjectsOfType<Transform>();
        foreach (Transform obj in allobj)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                //we use game component snapobject to identify if its a block...
                if (obj.GetComponent<SnapObject>() != null)
                {
                    GameObject.DestroyImmediate(obj.gameObject);
                }
            }
        }


    }
    public void LoadScene(string path)
    {
        List<Portal_And_Reference_Pair> portals = new List<Portal_And_Reference_Pair>();
        if (!EditorSceneManager.GetActiveScene().name.Contains("MasterGameScene"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MasterGameScene.unity");
        }
        else
        {
            ClearSceneBlocks();
        }
        scene = BinarySerializor.DeserializeFromBinary<Scene_Block>(path);

        Direction currentDirection = Direction.none;
        Transform grid = GameObject.Find("Grid").transform;
        GameObject objectToSpawn;
        int i = 0;
        foreach (block_data data in scene.blocks)
        {

            Debug.Log(i += 1);
            //identify the block we are working with.
            switch (data.type)
            {
                case blockType.goal:
                    objectToSpawn = goal_block;
                    break;
                case blockType.iron:
                    objectToSpawn = iron_block;
                    break;
                case blockType.one_way_b:
                    objectToSpawn = oneway_block_b;
                    currentDirection = data.direction;
                    break;
                case blockType.one_way_g:
                    objectToSpawn = oneway_block_g;
                    currentDirection = data.direction;
                    break;
                case blockType.portal:
                    objectToSpawn = portal_block;
                    break;
                case blockType.vortex:
                    objectToSpawn = vortex_block;
                    break;
                case blockType.walls:
                    objectToSpawn = wall_block;
                    break;
                case blockType.wooden:
                    objectToSpawn = wood_block;
                    break;
                default:
                    Debug.LogWarning("block type unknown");
                    objectToSpawn = null;
                    break;
            }
            GameObject obj = GameObject.Instantiate(objectToSpawn, new Vector3(data.pos.x, data.pos.y, data.pos.z), Quaternion.identity, grid);
            obj.transform.localScale = new Vector3(data.scale.x, data.scale.y, data.scale.z);
            if(currentDirection != Direction.none)
            {
                if (data.type == blockType.one_way_b)
                {
                    obj.GetComponent<OneWayBlock>().currentDirection = data.direction;
                }
                else if (data.type == blockType.one_way_g)
                {
                    obj.GetComponent<OneWayGate>().currentDirection = data.direction;
                }
            }
            if(data.type == blockType.portal)
            {
                portals.Add(new Portal_And_Reference_Pair(obj.GetComponent<Portal>(), new Vector3(data.portal_ref_pos.x,data.portal_ref_pos.y, data.portal_ref_pos.z)));
            }
        }
        //Handle Portals if any

        for (int count = 0; count < portals.Count; count++)
        {
            Portal otherPortal = null;
            for(int k = 0; k < portals.Count; k++)
            {
                if(portals[k].portal.transform.position == portals[count].Pos)
                {
                    otherPortal = portals[k].portal;
                    break;
                }
            }
            if(otherPortal == null)
            {
                Debug.LogWarning("Cannot find ref portal. Error!");
            }
            portals[count].portal.OtherPortal = otherPortal;
        }


        Debug.Log("Successfully loaded");
        Current_World = scene.world; Current_Level = scene.level;

        PlayerPrefs.SetInt("CurrentWorld", Current_World);
        PlayerPrefs.SetInt("CurrentLevel", Current_Level);
    }

    

    //[Inspect]
    public void UpdateWorldLevel()
    {
        Vector2 temp = updateinternalworldlevel();
        Current_World = (int)temp.x;
        Current_Level = (int)temp.y;
    }

    public Vector2 updateinternalworldlevel()
    {
        GameObject GameSystem = GameObject.FindGameObjectWithTag("GameController");
        GameSystem.GetComponent<ScoreSystem>().UpdateWorldLevelInts();
        return new Vector2(GameSystem.GetComponent<ScoreSystem>().World, GameSystem.GetComponent<ScoreSystem>().level); 
    }

    [Inspect, Descriptor("Save Puzzle","Save current changes to blocks in 'Grid'. Uses new_world and new_level to determine world/level. If its a deprecated scene, new world and level is not necesary as it will parse the name of the scene"),Spacing(2,2)]
    //Don't use this at runtime. PLEASE.
    public void SaveScene()
    {
        int world, level;

        //If the scenes are the deprecated scenes we are using.
        if (EditorSceneManager.GetActiveScene().name.Contains("Level"))
        {
            Vector2 temp = updateinternalworldlevel();
            world = (int)temp.x;
            level = (int)temp.y;
        }
        else
        {
            //this must be the new method, aka using the masterscene
            world = new_world;
            level = new_level;
            GameObject GameSystem = GameObject.FindGameObjectWithTag("GameController");
            GameSystem.GetComponent<ScoreSystem>().World = world;
            GameSystem.GetComponent<ScoreSystem>().level = level;
        }

        Scene_Block saveData = new Scene_Block();
        saveData.blocks = new List<block_data>();
        GameObject grid = GameObject.Find("Grid");

        //find all the blocks
        List<Transform> blks = new List<Transform>();
        
        Transform[] allobj = GameObject.FindObjectsOfType<Transform>();
        foreach (Transform obj in allobj)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                //we use game component snapobject to identify if its a block...
                if (obj.GetComponent<SnapObject>() != null || obj.CompareTag("PlacedWallBlock"))
                {
                    blks.Add(obj);
                }
            }
        }

        //create and place each new block_data
        foreach(Transform blk in blks)
        {
            block_data data = new block_data();
            data.pos = new custom_vector_4(blk.transform.position);
            data.scale = new custom_vector_4(blk.transform.localScale);
            data.type = ParseBlockType(blk.gameObject);

            //hardcore handle portals
            if(data.type == blockType.portal)
            {
                data.portal_ref_pos = new custom_vector_4(blk.GetComponent<Portal>().OtherPortal.transform.position);
            }

            if(blk.GetComponent<OneWayBlock>() != null)
            {
                data.direction = blk.GetComponent<OneWayBlock>().currentDirection;
            }
            else if(blk.GetComponent<OneWayGate>() != null)
            {
                data.direction = blk.GetComponent<OneWayGate>().currentDirection;
            }
            saveData.blocks.Add(data);
        }

        saveData.world = world; saveData.level = level;
        //now we actually save the data.
        if (BinarySerializor.SerializeToBinary<Scene_Block>(SaveParentFolder +"/" + world + "-" + level + ".dat", saveData))
            Debug.Log("Successfully saved");
    }

    private blockType ParseBlockType(GameObject obj)
    {
        if (obj.name.Contains("Goal"))
        {
            return blockType.goal;
        }
        else if (obj.name.Contains("Iron"))
        {
            return blockType.iron;
        }
        else if (obj.name.Contains("One Way Block"))
        {
            return blockType.one_way_b;
        }
        else if (obj.name.Contains("One Way Gate"))
        {
            return blockType.one_way_g;
        }
        else if (obj.name.Contains("Portal"))
        {
            return blockType.portal;
        }
        else if (obj.name.Contains("Vortex"))
        {
            return blockType.vortex;
        }
        else if (obj.name.Contains("Wall"))
        {
            return blockType.walls;
        }
        else if (obj.name.Contains("Wood"))
        {
            return blockType.wooden;
        }
        Debug.LogWarning("Object " + obj.name + " cannot be parsed. Error.");
        return blockType.none;
    }

    [Inspect,Spacing(10),Descriptor("Save All Scenes that are in build.","Scenes listed in Build Settings will be saved, only intended use is for deprecated scenes")]
    public void SaveAllBuildScene()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        foreach(EditorBuildSettingsScene scene in scenes)
        {
            //We check if the scene is the deprecated ones, marked by the keyword level. Only level selection defies this brand, so we check it out.
            if(scene.enabled && scene.path.Contains("Level") && !scene.path.Contains("Selection"))
            {
                Debug.Log("Scene located at " + scene.path + " is found to be old version and will be saved");
                EditorSceneManager.OpenScene(scene.path);
                SaveScene();
            }
        }
    }

}