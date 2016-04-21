using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
public class BlockMovement : MonoBehaviour {

	public Vector3 blockVel;

	Vector3 origBoxColSize;

	public Vector3 fallingBoxColSize;

	public float fallSpeedThreshold; //Must exceed falling speed before shrinking collision box
    Rigidbody rb;
    BoxCollider box;

	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        origBoxColSize = box.size;
	}

	// Update is called once per frame
	void Update () {
        blockVel = rb.velocity;

		if (Math.Abs(blockVel.y) > fallSpeedThreshold) {
            box.size = fallingBoxColSize;
		} 
		else {
            box.size = origBoxColSize;
            rb.velocity = Vector3.zero;
		}
	}
}
