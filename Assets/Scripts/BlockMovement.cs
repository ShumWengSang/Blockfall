using UnityEngine;
using System.Collections;
using System;

public class BlockMovement : MonoBehaviour {

	public Vector3 blockVel;

	Vector3 origBoxColSize;

	Vector3 fallingBoxColSize;

	public float fallSpeedThreshold; //Must exceed falling speed before shrinking collision box

	// Use this for initialization
	void Start () {
		origBoxColSize = GetComponent<BoxCollider>().size;

		fallingBoxColSize = GetComponent<BoxCollider>().size;
		fallingBoxColSize.x = 0.98f;
		fallingBoxColSize.y = 0.98f;

		fallSpeedThreshold = 0.15f;
	}


	// Update is called once per frame
	void Update () {
		blockVel = GetComponent<Rigidbody>().velocity;

		if (Math.Abs(blockVel.y) > fallSpeedThreshold) {
			GetComponent<BoxCollider>().size = fallingBoxColSize;
		} 
		else {
			GetComponent<BoxCollider>().size = origBoxColSize;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}
