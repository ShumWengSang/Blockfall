using UnityEngine;
using System.Collections;

public class BlockAnimator : MonoBehaviour {


    Transform thisTransform;
    void Start()
    {
        thisTransform = transform;
    }
	// Update is called once per frame
	void Update () {
        thisTransform.rotation = Quaternion.identity;
	}
}
