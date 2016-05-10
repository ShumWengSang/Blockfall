using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Lean;
using DG.Tweening;
using AdvancedInspector;
using UnityEngine.UI;
using System.Linq;

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
        GameObject[] ironBlocks = GameObject.FindGameObjectsWithTag("Iron Block");
        Blocks = new Rigidbody[listOfGO.Length + ironBlocks.Length];
        for (int i = 0; i < listOfGO.Length; i++)
        {
            Blocks[i] = listOfGO[i].GetComponent<Rigidbody>();
        }
        for(int i = 0; i < ironBlocks.Length; i++)
        {
            Blocks[i + listOfGO.Length] = ironBlocks[i].GetComponent<Rigidbody>();
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void ChangeBlocksMass()
    {
        //Not used anymore.
        Rigidbody[] orderedBlocks = Blocks.OrderBy(block => block.transform.position.y).ToArray();
        float mass = 1;
        for (int i = orderedBlocks.Length - 1; i >= 0; i--)
        {
            orderedBlocks[i].mass = ++mass;
        }
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
            if (Blocks[i].velocity.sqrMagnitude > 0.05f)
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

    public delegate void GamePhase();
    public static event GamePhase OnFinishedRotating;
    public static event GamePhase OnStartFalling;
    public static event GamePhase OnFinishedFalling;
    public static event GamePhase OnUndoFinish;
    public static event GamePhase OnUndoStart;

    public Button LeftButton;
    public Button RightButton;
    void SetButtonInteractable(bool interact)
    {
        LeftButton.interactable = interact;
        RightButton.interactable = interact;
    }
    enum GridOrientation
    {
        Left,
        Right
    }
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    enum Quadrant
    {
        First,
        Second,
        Third,
        Fourth
    }

    WaitForSeconds waitUndo;
    Stack<GridOrientation> LastMoves;
	Transform targetRotation;
    Camera mainCamera;
    Tween theTween;
    public Text MovesDone;
    public WoodenBlockManager woodBlockManager;
    Vector2 halfVector = new Vector2(0.5f, 0.5f);

	public float rotationTime = 1.0f;
    bool GameOver = false;
    bool FinishedFalling;

    public Animator expandAnimator;
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

        waitUndo = new WaitForSeconds(BlockUndoModule.undoTime);
        LastMoves = new Stack<GridOrientation>();
        mainCamera = Camera.main;
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
	// Use this for initialization
	void Start () {
		targetRotation = transform;
        TimesMoved = 0;
        SetButtonInteractable(true);

	}

    public void ButtonLeft()
    {
        if (!GameOver)
        {
            if (woodBlockManager.areBlocksStationary())
            {
                woodBlockManager.SetBlockKinemactics(true);
                RotateLeft();
            }
        }
    }

    public void ButtonRight()
    {
        if (!GameOver)
        {
            if (woodBlockManager.areBlocksStationary())
            {
                woodBlockManager.SetBlockKinemactics(true);
                RotateRight();
            }
        }
    }
    float timeLeft = 0.1f;

	void Update() {

#if UNITY_EDITOR
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
#endif

        if (woodBlockManager.areBlocksStationary() && !FinishedFalling)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                FinishedFalling = true;
                OnFinishedFalling();
                timeLeft = 0.1f;
            }
        }

	}

    void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
        GoalChecker.OnFinishedGame += OnGameComplete;
        RotateGrid.OnFinishedFalling += OnFinishedFallingDown;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        RotateGrid.OnFinishedFalling -= OnFinishedFallingDown;
        GoalChecker.OnFinishedGame -= OnGameComplete;
    }


    void OnFingerSwipe(LeanFinger finger)
    {
        if (!woodBlockManager.areBlocksStationary())
            return;
        Ray fingerRay = finger.GetStartRay();
        woodBlockManager.SetBlockKinemactics(true);

        var StartViewPos = mainCamera.ScreenToViewportPoint(finger.StartScreenPosition);
        var EndViewPos = mainCamera.ScreenToViewportPoint(finger.LastScreenPosition);

        if(StartViewPos.x < 0.5f && StartViewPos.y < 0.5f)
        {
            HandleQuadrant(Quadrant.Fourth, finger);
        }
        else if(StartViewPos.x < 0.5f && StartViewPos.y >= 0.5f)
        {
            HandleQuadrant(Quadrant.First, finger);
        }
        else if(StartViewPos.x >= 0.5f && StartViewPos.y < 0.5f)
        {
            HandleQuadrant(Quadrant.Third, finger);
        }
        else if(StartViewPos.x >= 0.5f && StartViewPos.y >= 0.5f)
        {
            HandleQuadrant(Quadrant.Second, finger);
        }
    }

    void HandleQuadrant(Quadrant theQuad, LeanFinger finger)
    {
        Direction swipeDirection = DetermineSwipeDirection(finger);
        if(swipeDirection == Direction.None)
        {
            Debug.LogWarning("Direction si null");
            return;
        }
        switch(theQuad)
        {
                //Top Half
            case Quadrant.First:
            case Quadrant.Second:
                if(swipeDirection == Direction.Up)
                {
                    if(theQuad == Quadrant.Second)
                    {
                        RotateLeft();
                    }
                    else
                    {
                        RotateRight();
                    }
                }
                else if(swipeDirection == Direction.Down)
                {
                    if (theQuad == Quadrant.Second)
                    {
                        RotateRight();
                    }
                    else
                    {
                        RotateLeft();
                    }
                }
                else if(swipeDirection == Direction.Left)
                {
                    RotateLeft();
                }
                else if(swipeDirection == Direction.Right)
                {
                    RotateRight();
                }
                break;
                //Bottom Half
            case Quadrant.Third:
            case Quadrant.Fourth:
                if(swipeDirection == Direction.Up)
                {
                    if(theQuad == Quadrant.Third)
                    {
                        RotateLeft();
                    }
                    else
                    {
                        RotateRight();
                    }
                }
                else if(swipeDirection == Direction.Down)
                {
                    if (theQuad == Quadrant.Fourth)
                    {
                        RotateLeft();
                    }
                    else
                    {
                        RotateRight();
                    }
                }
                else if(swipeDirection == Direction.Left)
                {
                    RotateRight();
                }
                else if(swipeDirection == Direction.Right)
                {
                    RotateLeft();
                }
                break;
        }
    }
    Direction DetermineSwipeDirection(LeanFinger finger)
    {
        var swipe = finger.SwipeDelta;

        if (swipe.x < -Mathf.Abs(swipe.y))
        {
            return Direction.Left;
        }

        if (swipe.x > Mathf.Abs(swipe.y))
        {
            return Direction.Right;
        }

        if (swipe.y < -Mathf.Abs(swipe.x))
        {
            return Direction.Down;
        }

        if (swipe.y > Mathf.Abs(swipe.x))
        {
            return Direction.Up;
        }
        return Direction.None;
    }
    void RotateLeft()
    {
        SetButtonInteractable(false);
        if (theTween == null)
        {
            woodBlockManager.AlignBlocks();
            {
                expandAnimator.SetTrigger("MoveLeft");
				this.GetComponent<PrintAnswer>().AddMoveToList("Left");

                TimesMoved++;
                LastMoves.Push(GridOrientation.Left);
                theTween = targetRotation.DORotate(new Vector3(0, 0, 90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
            }
        }
    }

    void RotateRight()
    {
        SetButtonInteractable(false);
        if (theTween == null)
        {
            woodBlockManager.AlignBlocks();
            {
                expandAnimator.SetTrigger("MoveRight");
				this.GetComponent<PrintAnswer>().AddMoveToList("Right");
                TimesMoved++;
                LastMoves.Push(GridOrientation.Right);
                theTween = targetRotation.DORotate(new Vector3(0, 0, -90), rotationTime).SetEase(Ease.OutSine).OnComplete(OnTweenComplete).SetRelative();
            }
        }
    }

    void OnUndoTweenComplete()
    {

        woodBlockManager.AlignBlocks();
        theTween = null;
        woodBlockManager.SetBlockKinemactics(false);
        TimesMoved--;
        if (OnUndoFinish != null) OnUndoFinish();
    }

    void OnTweenComplete()
    {
        if (OnFinishedRotating != null) OnFinishedRotating();
        woodBlockManager.AlignBlocks();
        woodBlockManager.SetBlockKinemactics(false);
        //woodBlockManager.ChangeBlocksMass();
        if (OnStartFalling != null) OnStartFalling();
        theTween = null;
        FinishedFalling = false;
    }

    public void UndoLastMove()
    {
        if(LastMoves.Count > 0)
            StartCoroutine(undoMove());
    }

    IEnumerator undoMove()
    {
        if(OnUndoStart != null) OnUndoStart();
        woodBlockManager.SetBlockKinemactics(true);
        BlockUndoModule.UndoAllBlocks();
        yield return waitUndo;

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
        theTween = targetRotation.DORotate(targetVector, rotationTime).SetEase(Ease.OutSine).OnComplete(OnUndoTweenComplete).SetRelative();
    }



    void OnGameComplete()
    {
        LeanTouch.Instance.enabled = false;
        GameOver = true;
    }

    void OnFinishedFallingDown()
    {
        SetButtonInteractable(true);
    }
}
