using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSoundPlayer : MonoBehaviour {

    AudioSource audioPlayer;
    public AudioClip clip;
	// Use this for initialization
	void Start () {
        audioPlayer = GetComponent<AudioSource>();
	}

    private void OnEnable()
    {
        Portal.OnPortalTransport += OnPortalTransport;
    }

    private void OnDisable()
    {
        Portal.OnPortalTransport -= OnPortalTransport;
    }

    private void OnPortalTransport()
    {
        audioPlayer.PlayOneShot(clip, GameSounds.GameSoundVolume);
    }
}
