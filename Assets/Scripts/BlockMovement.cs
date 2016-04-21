using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
public class BlockMovement : MonoBehaviour {

	public Vector3 blockVel;

	Vector3 origBoxColSize;

	public Vector3 fallingBoxColSize;

	public float fallSpeedThreshold; //Must exceed falling speed before shrinking collision box
    Rigidbody rb;
    BoxCollider box;
    Tween theTween;

    void OnEnable()
    {
        RotateGrid.OnFinishedRotating += OnStartFall;
        RotateGrid.OnFinishedFalling += OnEndFall;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedRotating -= OnStartFall;
        RotateGrid.OnFinishedFalling -= OnEndFall;
    }
	// Use this for initialization
	void Start () {
        box = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        origBoxColSize = box.size;
        theTween = transform.DOBlendableScaleBy(new Vector3(0.05f,0.05f,0f), 5f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
	}

    void OnEndFall()
    {
        theTween.Play(); 
    }

    void OnStartFall()
    {
        theTween.Pause();
        this.transform.localScale = Vector3.one;
        SnapObject.SnapTheObject(this.transform);
    }

	// Update is called once per frame
	void Update () {
        blockVel = rb.velocity;

		if (Math.Abs(blockVel.y) > fallSpeedThreshold) {
            box.size = fallingBoxColSize;
		} 
		else {
            box.size = origBoxColSize;
            rb.velocity = Vector3.zero;
		}
	}
}
