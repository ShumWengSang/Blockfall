using UnityEngine;
using System.Collections;

public class DampVelocity : MonoBehaviour {

    public float Damp = 0.1f;
    public float velLimit = 1f;
    Rigidbody thisRb;

    void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (thisRb.velocity.magnitude > velLimit)
            thisRb.velocity = new Vector3(thisRb.velocity.x, thisRb.velocity.y * (1 - Damp), thisRb.velocity.z);
	}
}
