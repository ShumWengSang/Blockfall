using UnityEngine;
using System.Collections;

public class GoalCheck : MonoBehaviour {

	// Use this for initialization


	float unlitEmission = 0.3f;
	float litEmission = 0.7f;
	Color origColor;
	bool goalScored;

	void Start () {
		foreach (Transform child in transform.parent) { //loop through borders
			if (child.name != this.name) {
				Material mat = child.GetComponent<Renderer> ().material; 
				origColor = mat.color;
				break;
			}
		}

		goalScored = false;
	}
		
	void OnTriggerEnter(Collider col) {
		if (col.tag == "WoodBlock") {
			SetEmission (litEmission);
			goalScored = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "WoodBlock") {
			SetEmission (unlitEmission);
			goalScored = false;
		}
	}

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
