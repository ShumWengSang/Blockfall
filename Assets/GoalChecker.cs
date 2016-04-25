using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GoalChecker : MonoBehaviour {

	//public RotateGrid grid;
	GameObject grid;
    GoalCheck []goalCheck;
    WoodenBlockManager manager;

    public delegate void GameComplete();
    public static event GameComplete OnFinishedGame;

    bool SentFinishedGame = false;

    void OnEnable()
    {
        RotateGrid.OnFinishedFalling += OnFinishFalling;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedFalling -= OnFinishFalling;
    }

	// Use this for initialization
	void Start () {
        SentFinishedGame = false;
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

    void OnFinishFalling()
    {
        if (areGoalsScored())
        {
            if (OnFinishedGame != null && !SentFinishedGame)
            {
                OnFinishedGame();
                SentFinishedGame = true;
                this.GetComponent<PrintAnswer>().WriteToFile("Answers/" + SceneManager.GetActiveScene().name + ".txt");
            }
        }
    }
}
