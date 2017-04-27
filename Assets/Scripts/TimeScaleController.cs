using UnityEngine;
using System.Collections;

public class TimeScaleController : MonoBehaviour {

    public float timeScale = 1.75f;

    public float timeScaleModifier = 0;
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

    public void OnModifyTimeScale(float newTime)
    {
        timeScaleModifier = newTime;
        Time.timeScale = timeScale + timeScaleModifier;
    }

    void OnFinishedGame()
    {
        Time.timeScale = 1f;
    }
}
