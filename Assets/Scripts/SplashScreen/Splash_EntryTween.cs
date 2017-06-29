using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash_EntryTween : MonoBehaviour {
    public GameObject wood_block_prefab;
    public GameObject goal_prefab;

    private void Awake()
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    GameObject spawnObject(GameObject obj)
    {
        return GameObject.Instantiate(obj);
    }
}
