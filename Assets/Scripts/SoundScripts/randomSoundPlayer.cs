using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSoundPlayer : MonoBehaviour
{
    public AudioClip[] arrayOfClips;

    public AudioClip GetRandomClip()
    {
        int counter = UnityEngine.Random.Range(0, arrayOfClips.Length);
        return arrayOfClips[counter];
    }
}