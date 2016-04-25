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

                if (System.IO.File.Exists("Answers/" + SceneManager.GetActiveScene().name + ".txt")) //if file exists
                {
                    //check if current answer is shorter
                    int prevAnsMoves = this.GetComponent<PrintAnswer>().GetNumberOfMoves(SceneManager.GetActiveScene().name + ".txt");
                    int currentAnsMoves = this.GetComponent<PrintAnswer>().movesMadeList.Count;

                    if (currentAnsMoves < prevAnsMoves)
                        this.GetComponent<PrintAnswer>().WriteToFile("Answers/" + SceneManager.GetActiveScene().name + ".txt");
                }
                else
                {
                    this.GetComponent<PrintAnswer>().WriteToFile("Answers/" + SceneManager.GetActiveScene().name + ".txt");
                }           
            }
        }
    }
}
