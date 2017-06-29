using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusicPlayer : MonoBehaviour {
    public static GlobalMusicPlayer instance = null;
    public randomSoundPlayer musics;
    AudioSource player;

	// Use this for initialization
	void Start () {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        player = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        player.PlayOneShot(musics.GetRandomClip());
	}
    private void Update()
    {
        if(!player.isPlaying)
        {
            player.PlayOneShot(musics.GetRandomClip());
        }
        player.volume = GameSounds.GameMusicVolume;
    }
}
