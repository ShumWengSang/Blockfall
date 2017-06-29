using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexBoxSoundPlayer : MonoBehaviour {
    public randomSoundPlayer randomSounds;
    private AudioSource audioPlayer;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {

        VortexBox.OnObjectHold += OnObjectHold;
    }

    private void OnDisable()
    {

        VortexBox.OnObjectHold -= OnObjectHold;
    }

    void OnObjectHold()
    {
        audioPlayer.PlayOneShot(randomSounds.GetRandomClip(), GameSounds.GameSoundVolume);
    }
}
