using UnityEngine;
using System.Collections;

public class ChangeWallTexture : MonoBehaviour {

    public Material[] wallTextures;

    GameObject GameSystem;

	// Use this for initialization
	void Start () {
        GameSystem = GameObject.FindGameObjectWithTag("GameController");

        GetComponent<MeshRenderer>().material = wallTextures[GameSystem.GetComponent<ScoreSystem>().World - 1];
    }
}