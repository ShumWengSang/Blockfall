using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GoalCheck : MonoBehaviour {

	// Use this for initialization


	float unlitEmission = 0.3f;
	float litEmission = 0.7f;
	Color origColor;
	public bool goalScored;

    //void Start () {
    //	foreach (Transform child in transform.parent) { //loop through borders
    //		if (child.name != this.name) {
    //			Material mat = child.GetComponent<Renderer> ().sharedMaterial; 
    //			origColor = mat.color;
    //			break;
    //		}
    //	}

    //	goalScored = false;
    //}

    private void Start()
    {
        foreach (Transform child in transform.parent)
        { //loop through borders
            if (child.name != this.name)
            {
                Material mat = child.GetComponent<Renderer>().sharedMaterial;
                origColor = mat.color;
                break;
            }
        }
    }

    float TempCurrentEmission = 0.1f;
    public float TempEmissionIncrement = 0.1f;
    public float MaxTempEmission = 0.7f;
    public float MinTempEmission = 0.3f;
    float TempEmisionDir = 1f;

    private void Update()
    {
        if(!goalScored)
        {
            TempCurrentEmission = TempCurrentEmission + TempEmissionIncrement * TempEmisionDir * Time.deltaTime;
            if(TempCurrentEmission >= MaxTempEmission)
            {
                TempCurrentEmission = MaxTempEmission;
                TempEmisionDir *= -1;
            }
            if(TempCurrentEmission <= MinTempEmission)
            {
                TempCurrentEmission = MinTempEmission;
                TempEmisionDir *= -1;
            }
            SetEmission(TempCurrentEmission);

        }
    }
    string wood = "WoodBlock";
    string endzone = "EndZone";
    void OnTriggerEnter(Collider col) {
		if (col.CompareTag(wood)) {
			SetEmission (litEmission);
			goalScored = true;
            col.GetComponentInChildren<Animator>().SetTrigger(endzone);
		}
	}

	void OnTriggerStay(Collider col) {
        if (col.CompareTag(wood))
        {
			SetEmission (litEmission);
			goalScored = true;
		}
	}

	void OnTriggerExit(Collider col) {
        if (col.CompareTag(wood))
        {
			SetEmission (unlitEmission);
			goalScored = false;
		}
	}

    string emissionColor = "_EmissionColor";

    void SetEmission(float emissionValue)
	{
        Material mat = transform.parent.GetChild(1).GetComponent<Renderer>().sharedMaterial;
        Color finalColor = origColor * emissionValue;
        mat.SetColor (emissionColor, finalColor);

        //foreach (Transform child in transform.parent) { //loop through borders
        //	if (child.name != this.name) {
        //		Material mat = child.GetComponent<Renderer> ().sharedMaterial; 
        //		Color finalColor = origColor * emissionValue;

        //		mat.SetColor (emissionColor, finalColor);
        //	}
        //}
    }
}
