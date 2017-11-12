using UnityEngine;
using System.Collections;

public class SetTargetFrameRate : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 30;
        Screen.SetResolution(800, 1280, true);
    }
}
