using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayTrophyAnimation : MonoBehaviour {

    public Sprite[] DontPlaySprites;
    bool dontplay = false;
	// Use this for initialization
	void Start () {
        //for(int i = 0; i < DontPlaySprites.Length; i++)
        {
            if(!transform.parent.GetComponent<Button>().IsInteractable())
            {
                dontplay = true;
                //break;
            }
        }
        if (dontplay)
        {
            GetComponent<Animator>().enabled = false;
        }
        else
        {
            GetComponent<Animator>().Play("Trophy_Movement_LevelSelector", -1, Random.Range(0f, 1f));
        }
	}
	
}
