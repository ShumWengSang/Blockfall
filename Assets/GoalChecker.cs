using UnityEngine;
using System.Collections;

public class GoalChecker : MonoBehaviour {

	//public RotateGrid grid;
	GameObject grid;
    GoalCheck []goalCheck;
    WoodenBlockManager manager;


	// Use this for initialization
	void Start () {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("GoalZone");
        goalCheck = new GoalCheck [goals.Length];
        for(int i = 0; i < goalCheck.Length; i++)
        {
            goalCheck[i] = goals[i].GetComponent<GoalCheck>();
        }
        manager = WoodenBlockManager.instance;
	}

	public bool areGoalsScored()
	{
        for (int i = 0; i < goalCheck.Length; i++)
		{
            if (goalCheck[i].goalScored == false)
				return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () {
		if (manager.areBlocksStationary ()) {
			if (areGoalsScored ()) {
				Debug.Log ("LEVEL COMPLETE");
			}
		}
	}
}
