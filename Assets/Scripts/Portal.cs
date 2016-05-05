using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public GameObject OtherPortal;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y - transform.position.y > 0) //Check if above
            other.transform.position = OtherPortal.transform.position;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
