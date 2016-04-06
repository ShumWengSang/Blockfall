using UnityEngine;
using System.Collections;

public class GoalCheck : MonoBehaviour {

	// Use this for initialization


	float unlitEmission = 0.3f;
	float litEmission = 0.7f;
	Color origColor;

	void Start () {
		foreach (Transform child in transform.parent) { //loop through borders
			if (child.name != this.name) {
				Material mat = child.GetComponent<Renderer> ().material; 
				origColor = mat.color;
				break;
			}
		}
	}
		
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray landingRay = new Ray (transform.position, Vector3.forward);

		if (Physics.Raycast (landingRay, out hit, 10f)) {
			if (hit.collider.tag == "WoodBlock") {
				SetEmission (litEmission);
			} else {
				SetEmission (unlitEmission);
			}
		}
		else {
			SetEmission (unlitEmission);
		}

	}//update



	void SetEmission(float emissionValue)
	{
		foreach (Transform child in transform.parent) { //loop through borders
			if (child.name != this.name) {
				Material mat = child.GetComponent<Renderer> ().material; 
				Color finalColor = origColor * emissionValue;

				mat.SetColor ("_EmissionColor", finalColor);
			}
		}
	}

}
