using UnityEngine;
using System.Collections;
using System;

public class BlockMovement : MonoBehaviour {

	public Vector3 blockVel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		blockVel = GetComponent<Rigidbody>().velocity;
		/*
		if (blockVel.x == 0 && blockVel.y == 0) {
			Vector3 positionVec = transform.position;
			positionVec.x = (float)Math.Round(positionVec.x, MidpointRounding.AwayFromZero) / 2;
			positionVec.y = (float)Math.Round(positionVec.x, MidpointRounding.AwayFromZero) / 2;

			transform.position = positionVec;
		}
		*/

		if ((Math.Abs (blockVel.x) < 0.01) && (Math.Abs (blockVel.y) < 0.01)) {
			
		}
	}
}
