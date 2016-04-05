using UnityEngine;
using System;
using System.Collections;
using Lean;
using DG.Tweening;
using AdvancedInspector;
using UnityEngine.UI;
public class RotateGrid : MonoBehaviour {

	Transform targetRotation;

	public float rotationTime = 1.0f;
    [Inspect(InspectorLevel.Debug)]
    public int TimesMoved
    {
        get { return MovedTime; }
        set
        {
            MovesDone.text = "Moves: " + value.ToString();
            MovedTime = value;
        }
    }

    public int MovedTime;
    Camera mainCamera;

    Vector2 halfVector = new Vector2(0.5f, 0.5f);

    Rigidbody[] Blocks;

    Tween theTween;
    public Text MovesDone;

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
		targetRotation = transform;
        TimesMoved = 0;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (areBlocksStationary ()) {
				SetBlockKinemactics (true);
				RotateLeft ();
			}
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (areBlocksStationary ()) {
				SetBlockKinemactics (true);
				RotateRight();
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			SetBlockKinemactics (true);
			AlignBlocks();
		}
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
            if (Blocks[i].velocity.magnitude > 0.01)
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

	void AlignBlocks()
	{
		for (int i = 0; i < Blocks.Length; i++)
		{
			Vector3 newPosition = Blocks[i].GetComponentInParent<Transform>().position;

			newPosition.x = Mathf.Round(newPosition.x * 2f) * 0.5f;
			newPosition.y = Mathf.Round(newPosition.y * 2f) * 0.5f;

			Blocks[i].GetComponentInParent<Transform>().position = newPosition;
		}
	}

    void RotateLeft()
    {
		if (theTween == null) {
			AlignBlocks();
        {
            TimesMoved++;
            theTween = targetRotation.DORotate(new Vector3(0, 0, 90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
        }
		}
	}

    void RotateRight()
    {
		if (theTween == null) {
			AlignBlocks();
        {
            TimesMoved++;
            theTween = targetRotation.DORotate(new Vector3(0, 0, -90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
        }
		}
	}

    void OnTweenComplete()
    {
        SetBlockKinemactics(false);
        theTween = null;
    }
}
