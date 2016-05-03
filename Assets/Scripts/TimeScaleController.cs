using UnityEngine;
using System.Collections;

public class TimeScaleController : MonoBehaviour {

    public float timeScale = 2.5f;
	// Use this for initialization
	void Start () {
        Time.timeScale = timeScale;
	}
    
    void OnEnable()
    {
        GoalChecker.OnFinishedGame += OnFinishedGame;
    }

    void OnDisable()
    {
        GoalChecker.OnFinishedGame -= OnFinishedGame;
    }

    void OnFinishedGame()
    {
        Time.timeScale = 1f;
    }
}
