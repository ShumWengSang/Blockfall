using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class GoalChecker : MonoBehaviour {

	//public RotateGrid grid;
	GameObject grid;
    GoalCheck []goalCheck;
    WoodenBlockManager manager;

    public delegate void GameComplete();
    public static event GameComplete OnFinishedGame;

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
				if (OnFinishedGame != null) {
					OnFinishedGame ();
					this.GetComponent<PrintAnswer>().WriteToFile("Answers/" + EditorSceneManager.GetActiveScene().name + ".txt");
				}
			}
		}
	}
}
