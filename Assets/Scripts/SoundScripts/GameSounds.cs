using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSounds : MonoBehaviour {
    public static float GameSoundVolume;
    public static float GameMusicVolume;
    public AudioClip gameOverSound;
    AudioSource auPlayer;
    public Slider soundVolume;
    public Slider musicVolume;

    public AudioClip sampleSound;

    public void SetSoundVolume(float input)
    {
        input *= 0.1f;
        GameSoundVolume = input;
        auPlayer.PlayOneShot(sampleSound, GameSoundVolume);
    }

    public void SetMusicVolume(float input)
    {
        GameMusicVolume = input;
    }

    private void OnEnable()
    {
        GoalChecker.OnFinishedGame += OnFinishedGame;
        GameSoundVolume = PlayerPrefs.GetFloat("GameSoundVolume", 1);
        GameMusicVolume = PlayerPrefs.GetFloat("GameMusicVolume", 1);
        soundVolume.value = GameSoundVolume * 10;
        musicVolume.value = GameMusicVolume;

    }

    private void OnDisable()
    {
        GoalChecker.OnFinishedGame -= OnFinishedGame;
        PlayerPrefs.SetFloat("GameSoundVolume", GameSoundVolume);
        PlayerPrefs.SetFloat("GameMusicVolume", GameMusicVolume);
    }

    // Use this for initialization
    void Awake () {
        auPlayer = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void OnFinishedGame () {
        auPlayer.PlayOneShot(gameOverSound, GameSoundVolume);
	}
}
