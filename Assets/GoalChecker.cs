using UnityEngine;
using System.Collections;

public class GoalChecker : MonoBehaviour {

	//public RotateGrid grid;
	GameObject grid;
	GameObject[] goals;

	// Use this for initialization
	void Start () {
		grid = GameObject.FindGameObjectWithTag("Grid");
		goals = GameObject.FindGameObjectsWithTag("GoalZone");
	}

	public bool areGoalsScored()
	{
		for (int i = 0; i < goals.Length; i++)
		{
			if (goals[i].GetComponent<GoalCheck>().goalScored == false)
				return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () {
		if (grid.GetComponent<RotateGrid> ().woodBlockManager.areBlocksStationary ()) {
			if (areGoalsScored ()) {
				Debug.Log ("LEVEL COMPLETE");
			}
		}
	}
}
