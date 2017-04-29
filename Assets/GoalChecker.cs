using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class GoalChecker : MonoBehaviour {

	//public RotateGrid grid;
	Transform grid;
    RotateGrid rotateGrid;
    List<GoalCheck> goalCheck;


    public delegate void GameComplete();
    public static event GameComplete OnFinishedGame;

    bool SentFinishedGame = false;

    public bool RecordAnswer = false;

    void OnEnable()
    {
        RotateGrid.OnFinishedFalling += OnFinishFalling;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedFalling -= OnFinishFalling;
    }

    private void Start()
    {
        Init();
    }

    // Use this for initialization
    void Init () {
        SentFinishedGame = false;
        GameObject[] goals = GameObject.FindGameObjectsWithTag("GoalZone");
        goalCheck = new List<GoalCheck>();
        grid = GameObject.Find("Grid").transform;
        rotateGrid = grid.GetComponent<RotateGrid>();
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].transform.IsChildOf(grid))
            {
                goalCheck.Add(goals[i].GetComponent<GoalCheck>());
            }
        }
        
	}

	public bool areGoalsScored()
	{
        for (int i = 0; i < goalCheck.Count; i++)
		{
            if (goalCheck[i].goalScored == false)
				return false;
		}
		return true;
	}

    //Hack to make sure goal is called since not sure where bug where goal is not called orginates from.
    IEnumerator checkGoalsAgain()
    {
        WaitForSeconds eachSecond = new WaitForSeconds(3f);
        yield return eachSecond;
        if(rotateGrid.woodBlockManager.areBlocksStationary())
            HandleFinishFalling();
    }

    void HandleFinishFalling()
    {
        if (areGoalsScored())
        {
            if (OnFinishedGame != null && !SentFinishedGame)
            {
                OnFinishedGame();
                SentFinishedGame = true;
                if (RecordAnswer)
                {
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

    void OnFinishFalling()
    {
        StartCoroutine(checkGoalsAgain());
        HandleFinishFalling();
    }
}
