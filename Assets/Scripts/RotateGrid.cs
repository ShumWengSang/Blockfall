using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Lean;
using DG.Tweening;
using AdvancedInspector;
using UnityEngine.UI;

public class WoodenBlockManager
{
    public static WoodenBlockManager instance;
    public Rigidbody[] Blocks;

    public WoodenBlockManager ()
    {
        Awake();
        Start();
    }
    void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        GameObject[] listOfGO = GameObject.FindGameObjectsWithTag("WoodBlock");
        Blocks = new Rigidbody[listOfGO.Length];
        for (int i = 0; i < listOfGO.Length; i++)
        {
            Blocks[i] = listOfGO[i].GetComponent<Rigidbody>();
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void SetBlockKinemactics(bool value)
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].isKinematic = value;
        }
    }

    public bool areBlocksStationary()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            if (Blocks[i].velocity.magnitude > 0.01)
                return false;
        }
        return true;
    }

    public bool areBlocksMoving()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            if (Blocks[i].velocity.magnitude == 0)
                return false;
        }
        return true;
    }

    public void AlignBlocks()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            Vector3 newPosition = Blocks[i].GetComponentInParent<Transform>().position;

            newPosition.x = Mathf.Round(newPosition.x * 2f) * 0.5f;
            newPosition.y = Mathf.Round(newPosition.y * 2f) * 0.5f;

            Blocks[i].GetComponentInParent<Transform>().position = newPosition;
        }
    }
}
public class RotateGrid : MonoBehaviour {
    enum GridOrientation
    {
        Left,
        Right
    }
    Stack<GridOrientation> LastMoves;
	Transform targetRotation;
    Camera mainCamera;
    Tween theTween;
    public Text MovesDone;
    public WoodenBlockManager woodBlockManager;
    Vector2 halfVector = new Vector2(0.5f, 0.5f);

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



    void Awake()
    {
        LastMoves = new Stack<GridOrientation>();
        mainCamera = Camera.main;
    }
	// Use this for initialization
	void Start () {
		targetRotation = transform;
        TimesMoved = 0;
        woodBlockManager = WoodenBlockManager.instance;
        if(woodBlockManager == null)
        {
            woodBlockManager = new WoodenBlockManager();
        }
        else
        {
            woodBlockManager.Start();
        }
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (woodBlockManager.areBlocksStationary())
            {
                woodBlockManager.SetBlockKinemactics(true);
				RotateLeft ();
			}
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (woodBlockManager.areBlocksStationary())
            {
                woodBlockManager.SetBlockKinemactics(true);
				RotateRight();
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
            woodBlockManager.SetBlockKinemactics(true);
            woodBlockManager.AlignBlocks();
		}
	}

    void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
        GoalChecker.OnFinishedGame += OnGameComplete;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        //GoalChecker.OnFinishedGame -= OnGameComplete;
    }


    void OnFingerSwipe(LeanFinger finger)
    {
        if (!woodBlockManager.areBlocksStationary())
            return;

        woodBlockManager.SetBlockKinemactics(true);

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
                woodBlockManager.SetBlockKinemactics(false);
            }
        }
    }
    void RotateLeft()
    {
        if (theTween == null)
        {
            woodBlockManager.AlignBlocks();
            {
                TimesMoved++;
                LastMoves.Push(GridOrientation.Left);
                theTween = targetRotation.DORotate(new Vector3(0, 0, 90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
            }
        }
    }

    void RotateRight()
    {
        if (theTween == null)
        {
            woodBlockManager.AlignBlocks();
            {
                TimesMoved++;
                LastMoves.Push(GridOrientation.Right);
                theTween = targetRotation.DORotate(new Vector3(0, 0, -90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
            }
        }
    }

    void OnTweenComplete()
    {
        woodBlockManager.SetBlockKinemactics(false);
        theTween = null;
    }

    public void UndoLastMove()
    {
        if(LastMoves.Count > 1)
            StartCoroutine(undoMove());
    }

    IEnumerator undoMove()
    {

        Physics.gravity = -Physics.gravity;
        yield return new WaitForSeconds(0.5f);
        while (!woodBlockManager.areBlocksStationary())
        {
            yield return null;
        }
        Physics.gravity = -Physics.gravity;
        woodBlockManager.SetBlockKinemactics(true);
        Vector3 targetVector;
        GridOrientation currentOrientation = LastMoves.Pop();
        if(currentOrientation == GridOrientation.Left)
        {
            targetVector = new Vector3(0, 0, -90);
            currentOrientation = GridOrientation.Right;
        }
        else
        {
            targetVector = new Vector3(0, 0, 90);
        }
        theTween = targetRotation.DORotate(targetVector, rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
    }

    void OnGameComplete()
    {
        LeanTouch.Instance.enabled = false;
    }
}
