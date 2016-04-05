using UnityEngine;
using System.Collections;
using Lean;
using DG.Tweening;

public class RotateGrid : MonoBehaviour {

	public bool rotatingGrid;

	Transform targetRotation;

	public float rotationSpeed;

    Camera mainCamera;

    Vector2 halfVector = new Vector2(0.5f, 0.5f);

    Rigidbody[] Blocks;

    Tween theTween;

    void Awake()
    {
        mainCamera = Camera.main;
        GameObject [] listOfGO = GameObject.FindGameObjectsWithTag("WoodBlock");
        Blocks = new Rigidbody[listOfGO.Length];
        for(int i = 0; i < listOfGO.Length; i++)
        {
            Blocks[i] = listOfGO[i].GetComponent<Rigidbody>();
        }
    }
	// Use this for initialization
	void Start () {
		rotatingGrid = false;
		targetRotation = transform;
	}

    void OnEnable()
    {
        //LeanTouch.OnFingerDown += OnFingerDown;
        //LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }

    void OnDisable()
    {
        //LeanTouch.OnFingerDown -= OnFingerDown;
        //LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
    }

    void OnFingerUp(LeanFinger finger)
    {

    }
    void OnFingerDown(LeanFinger finger)
    {
        
    }

    void SetBlockKinemactics(bool value)
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].isKinematic = value;
        }
    }

    bool areBlocksStationary()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            if (Blocks[i].velocity.magnitude != 0)
                return false;
        }
        return true;
    }

    bool areBlocksMoving()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            if (Blocks[i].velocity.magnitude == 0)
                return false;
        }
        return true;
    }

    void OnFingerSwipe(LeanFinger finger)
    {
        if (!areBlocksStationary())
            return;
        SetBlockKinemactics(true);
        var StartViewPos = mainCamera.ScreenToViewportPoint(finger.StartScreenPosition);
        var EndViewPos = mainCamera.ScreenToViewportPoint(finger.LastScreenPosition);

        if(StartViewPos.x >= 0.5f && EndViewPos.x >= 0.5f)
        {
            //Both are greater. This means its oing to the right.
            Debug.Log("Rotating right");
            RotateRight();
        }
        else if(StartViewPos.x < 0.5f && EndViewPos.x < 0.5f)
        {
            Debug.Log("Rotating left");
            RotateLeft();
        }
        else
        {
            //We check the direction of the swipe here to determine if it is a left or right swipe.
            //If it is a up or down swipe we ignore.
            var swipe = finger.SwipeDelta;

            if (swipe.x < -Mathf.Abs(swipe.y))
            {
                //Swipe left
                RotateLeft();
            }

            else if (swipe.x > Mathf.Abs(swipe.y))
            {
                //Swipe right
                RotateRight();
            }

            else
            {
                SetBlockKinemactics(false);
            }
        }
    }

    void RotateLeft()
    {
        if (theTween == null)
            theTween = targetRotation.DORotate(new Vector3(0, 0, 90), 1.0f).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
    }

    void RotateRight()
    {
        if (theTween == null)
            theTween = targetRotation.DORotate(new Vector3(0, 0, -90), 1.0f).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
    }

    void OnTweenComplete()
    {
        SetBlockKinemactics(false);
        theTween = null;
    }
}
