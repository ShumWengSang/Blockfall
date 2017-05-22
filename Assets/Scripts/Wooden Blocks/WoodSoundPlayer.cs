using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class WoodSoundPlayer : MonoBehaviour {
    public AudioClip[] clipsOfSounds;
    public AudioClip WoodHitWall;
    public AudioClip WoodHitVortex;
    public AudioClip WoodHitPortal;

    private AudioSource audioSource;
    private Vector3 blockVel;
    private Rigidbody rb;

    bool sound_played;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        sound_played = true;
        OnStartFalling();
	}

    private void OnEnable()
    {
        RotateGrid.OnStartFalling += OnStartFalling;
        RotateGrid.OnFinishedFalling += OnFinishedFalling;
    }
    private void OnDisable()
    {
        RotateGrid.OnStartFalling -= OnStartFalling;
        RotateGrid.OnFinishedFalling -= OnFinishedFalling;
    }

    void OnFinishedFalling()
    {
    }

    void OnStartFalling()
    {
        StartCoroutine(checkSpeed());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down);
    }

    private void Update()
    {
        blockVel = rb.velocity;

        if (rb.velocity.magnitude < 0.01f)
        {
            RaycastHit outInfo;
            if(Physics.Raycast(transform.position, Vector3.down, out outInfo, 1f))
            {
                SortRaycastHit(outInfo);
            }
        }
    }
    string placedwall = "PlacedWallBlock";
    string wall = "Wall";
    string ironwall = "Iron Block";
    string wood = "WoodBlock";
    string vortex = "Vortex";

    private void SortRaycastHit(RaycastHit info)
    {
        if(info.collider.CompareTag(placedwall) || info.collider.CompareTag(wall)
            || info.collider.CompareTag(ironwall) || info.collider.CompareTag(wood))
        {
            //We hit the wall. Lets play a sound.
            PlaySound(WoodHitWall);
        }
        else if(info.collider.CompareTag(vortex))
        {

        }
    }

    private AudioClip getClip(string name)
    {
        foreach (AudioClip clip in clipsOfSounds)
        {
            if(clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }

    private void PlaySound(AudioClip sound)
    {
        if(!sound_played)
        {
            sound_played = true;
            audioSource.PlayOneShot(sound);
        }
    }

    //threshold is 0.15
    private IEnumerator checkSpeed()
    {
        while(sound_played)
        {
            if(rb.velocity.magnitude > 1)
            {  
                 sound_played = false;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
