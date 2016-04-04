using UnityEngine;
using System.Collections;

public class RotateGrid : MonoBehaviour {

	public bool rotatingGrid;

	public Transform targetRotation;

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		rotatingGrid = false;
		rotationSpeed = 0.001f;
		targetRotation = transform;
	}

	void InitRotate()
	{
		rotatingGrid = true;
		GameObject.FindGameObjectsWithTag ("WoodBlock");
	}

	// Update is called once per frame
	void Update () {

		if (!rotatingGrid) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Vector3 rotationVec = targetRotation.rotation.eulerAngles;
				rotationVec.z = rotationVec.z + 90;
				targetRotation.rotation = Quaternion.Euler (rotationVec);

				rotatingGrid = true;
			}

			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Vector3 rotationVec = targetRotation.rotation.eulerAngles;
				rotationVec.z = rotationVec.z - 90;
				targetRotation.rotation = Quaternion.Euler (rotationVec);

				rotatingGrid = true;
			}
		} 
		else {
			float step = rotationSpeed * Time.deltaTime;

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation.rotation, 1.0f);

			if (transform.rotation.z == targetRotation.rotation.z) {
				rotatingGrid = false;
				Mathf.RoundToInt (transform.rotation.z);
			}
		}


	} //Update


}
