using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TimeScaleController : MonoBehaviour {

    public float timeScale = 1.75f;
    public Slider timeSlider;
    public float timeScaleModifier = 0;
	// Use this for initialization
	void Start () {
        timeScaleModifier = PlayerPrefs.GetFloat("TimeScaleDelta", timeScale);
        timeSlider.value = timeScaleModifier;
        Time.timeScale = timeScale + timeScaleModifier;
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
        PlayerPrefs.SetFloat("TimeScaleDelta", newTime);
    }

    void OnFinishedGame()
    {
        Time.timeScale = 1f;
    }
}
