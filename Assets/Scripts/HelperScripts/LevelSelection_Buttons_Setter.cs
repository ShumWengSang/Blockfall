using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedInspector;
using UnityEngine.UI;

public class LevelSelection_Buttons_Setter : MonoBehaviour {

    public SceneChanger sc;

    [Group("Setup Buttons Property"), Inspect]
    public int world;

    [Inspect]
    public void SetupButtons()
    {
        //We assume all the gameobjects are already there, cos they are (in my current work)
        //So we don't bother with checking or anything
        //max_levels == transform.childCount FYI
        int max_levels = transform.childCount;
        
        for(int i = 0; i < max_levels; i++)
        {
            int level = i;
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => sc.Load_Scene_Directed(world, level + 1));
        }     
    }

    private void Start()
    {
        SetupButtons();
    }
}
