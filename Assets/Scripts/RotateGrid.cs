#define NEW_METHOD
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Lean;
using DG.Tweening;
using AdvancedInspector;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

#region woodManager
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

            if (SnapObject.IsThisInteger(newPosition.x))
            {
                // do stuff
                newPosition.x += 0.5f;
            }
            if (SnapObject.IsThisInteger(newPosition.y))
            {
                newPosition.y += 0.5f;
                //do stuff
            }

            Blocks[i].GetComponentInParent<Transform>().position = newPosition;
        }
    }
}
#endregion
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
    #region enum
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
    #endregion

    private WaitForSeconds waitUndo;
    private Stack<GridOrientation> LastMoves;
    private Transform targetRotation;
    private Camera mainCamera;
    private Tween theTween;
    private PrintAnswer answer;

    public Text MovesDone;
    public WoodenBlockManager woodBlockManager;
    

	public float rotationTime = 1.0f;
    private bool GameOver = false;
    private bool FinishedFalling;

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
        answer = GetComponent<PrintAnswer>();
    }
	// Use this for initialization
	void Start () {
		targetRotation = transform;
        TimesMoved = 0;
        SetButtonInteractable(true);

#if NEW_METHOD


#endif
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
#if OLD_METHOD
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
#endif
        GoalChecker.OnFinishedGame += OnGameComplete;
        RotateGrid.OnFinishedFalling += OnFinishedFallingDown;
#if NEW_METHOD
        //LeanTouch.OnFingerDrag += OnFingerDrag;
#endif
    }

    void OnDisable()
    {
#if OLD_METHOD
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
#endif
        RotateGrid.OnFinishedFalling -= OnFinishedFallingDown;
        GoalChecker.OnFinishedGame -= OnGameComplete;
#if NEW_METHOD
        //LeanTouch.OnFingerDrag -= OnFingerDrag;
#endif
    }

    public void PauseFinger()
    {
        pauseFinger = !pauseFinger;
    }

    public bool pauseFinger = false;
#if NEW_METHOD
    #region DragData
    private Vector2 StartDrag_ScreenPos;
    private Vector2 EndDrag_ScreenPos;

    //the middle of the grid rests at 0.6 Y
    private Vector2 middle = new Vector2(0.5f, 0.6f);
    private Vector3 initialRotation;
    private float initialAngle = 0f;
    private float stepIncrease = 0f;
    private float totalStep = 0;
    public float StepIncrementValue = 1f;
    public float TweenDuration = 0.5f;
    public Vector3 initialAngleVector3;
    private bool TweenMoving = false;
    private bool Once_Instance_Of_Dragging_Flag = false;
    [RangeValue(-20f, 20f)]
    public RangeFloat Direction_Determinator = new RangeFloat(-10f, 10f);
    float lastAngle = 0.0f;
    float lastDir = 1.0f;
    public Transform DragResponseAnim;
    private int Current_Pointer_ID = -1;
    #endregion

    public float GetRadians(Vector2 point, Vector2 referencePoint)
    {
        return Mathf.Atan2(point.x - referencePoint.x, point.y - referencePoint.y);
        ;
    }
     
    public float GetDegrees(Vector2 point,Vector2 referencePoint)
    {
        return GetRadians(point, referencePoint) * Mathf.Rad2Deg;
    }

    public void OnFingerDrag(PointerEventData data)
    {

        if (TweenMoving)
        {
            return;
        }
        if(Current_Pointer_ID != data.pointerId)
        {
            return;
        }
        float newAngle = GetDegrees(mainCamera.ScreenToViewportPoint(data.position), middle);


        //swap the two variables around in the formula ; quick optimization because it will go in opposite direction
        // totalStep = lastAngle - newAngle;
        totalStep = initialAngle - newAngle;
        //Adding this causes the bug since clamping it makes it bug.
        if(totalStep < -180)
        {
            totalStep += 360;
        }
        else if (totalStep > 180)
        {
            totalStep -= 360;
        }

        
        if (Mathf.Abs(lastAngle) > 170)
        {
            if(lastAngle > 0)
            {
                if(totalStep < 0)
                {
                    totalStep += 360;
                }
            }
            else
            {
                if (totalStep > 0)
                {
                    totalStep -= 360;
                }
            }
        }

        lastAngle = totalStep;
        totalStep = Mathf.Clamp(totalStep, -20, 20);
        
        //slow down the thing after clamping if exceed clamp angle
        if(totalStep >= 20 || totalStep <= -20)
        {
            stepIncrease += StepIncrementValue * Time.deltaTime;
            totalStep += Mathf.Clamp(totalStep, -1, 1) * stepIncrease;
        }
        lastDir = Mathf.Clamp(lastAngle, -1, 1);

        Quaternion temp = new Quaternion();
        temp.eulerAngles = new Vector3(initialRotation.x, initialRotation.y, initialRotation.z + totalStep);
        //transform.localRotation = temp;
        //transform.Rotate(Vector3.forward, totalStep);
        transform.DORotate(temp.eulerAngles, 0.1f).SetEase(Ease.OutBounce);


    }

    public Ease animateEase;
    public void AnimateDrag_PointDown(PointerEventData data)
    {
        DragResponseAnim.gameObject.SetActive(true);
        DragResponseAnim.localScale = Vector3.zero;
        DragResponseAnim.position = mainCamera.ScreenToWorldPoint(data.position);
        DragResponseAnim.position = new Vector3(DragResponseAnim.position.x, DragResponseAnim.position.y, 0);
        DragResponseAnim.DOScale(20f, 0.5f).OnComplete( ()=> {DragResponseAnim.gameObject.SetActive(false); }).SetEase(animateEase);
    }
    
    public void OnStartDrag(PointerEventData data)
    {
        if (TweenMoving)
            return;
        if(Current_Pointer_ID != -1)
        {
            return;
        }
        Current_Pointer_ID = data.pointerId;

        StartDrag_ScreenPos = mainCamera.ScreenToViewportPoint(data.position);
        initialAngle = GetDegrees(StartDrag_ScreenPos, middle);
        initialRotation = transform.localRotation.eulerAngles;
        woodBlockManager.SetBlockKinemactics(true);
        totalStep = 0;
        Once_Instance_Of_Dragging_Flag = true;

        initialAngleVector3 = transform.localEulerAngles;
    }

    public void OnEndDrag(PointerEventData data)
    {
        if(data.pointerId != Current_Pointer_ID)
        {
            return;
        }
        EndDrag_ScreenPos = mainCamera.ScreenToViewportPoint(data.position);
        stepIncrease = 0;
        Current_Pointer_ID = -1;
        if (totalStep >= Direction_Determinator.min && totalStep <= Direction_Determinator.max)
        {
            //This means the user has not moved enough to justify a move, so we move the grid back.
            transform.DORotate(initialAngleVector3, TweenDuration).SetEase(Ease.OutBounce).OnComplete(
                () => { woodBlockManager.SetBlockKinemactics(false); TweenMoving = false; });
        }
        else
        {
            float direction = Mathf.Clamp(totalStep, -1, 1);

            SetButtonInteractable(false);
            //woodBlockManager.AlignBlocks();

            if (direction == -1)
            {
                expandAnimator.SetTrigger("MoveRight");
                answer.AddMoveToList("Right");
                LastMoves.Push(GridOrientation.Right);
            }
            else
            {
                expandAnimator.SetTrigger("MoveLeft");
                answer.AddMoveToList("Left");
                LastMoves.Push(GridOrientation.Right);
            }

            TimesMoved++;
 

            //the user has moved far enough to move, so we determine whether we move left or right.

            Vector3 finalAngleVector = new Vector3(initialAngleVector3.x, initialAngleVector3.y, initialAngleVector3.z + (direction * 90));
            transform.DORotate(finalAngleVector, TweenDuration).SetEase(Ease.OutSine).OnComplete(
                () => { OnTweenComplete(); TweenMoving = false; });

        }
        TweenMoving = true;
    }

#endif

#if OLD_METHOD
    void OnFingerSwipe(LeanFinger finger)
    {
        if (!woodBlockManager.areBlocksStationary())
            return;
        if (pauseFinger)
            return;
        woodBlockManager.SetBlockKinemactics(true);

        var StartViewPos = mainCamera.ScreenToViewportPoint(finger.StartScreenPosition);
        //var EndViewPos = mainCamera.ScreenToViewportPoint(finger.LastScreenPosition);

        if(StartViewPos.x < 0.5f && StartViewPos.y < 0.6f)
        {
            HandleQuadrant(Quadrant.Fourth, finger);
        }
        else if(StartViewPos.x < 0.5f && StartViewPos.y >= 0.6f)
        {
            HandleQuadrant(Quadrant.First, finger);
        }
        else if(StartViewPos.x >= 0.5f && StartViewPos.y < 0.6f)
        {
            HandleQuadrant(Quadrant.Third, finger);
        }
        else if(StartViewPos.x >= 0.5f && StartViewPos.y >= 0.6f)
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


#endif

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
                answer.AddMoveToList("Left");

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
                answer.AddMoveToList("Right");
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

    static public float undoWaitExtra = 0.0f;
    IEnumerator undoMove()
    {
        if(OnUndoStart != null) OnUndoStart();
        woodBlockManager.SetBlockKinemactics(true);
        BlockUndoModule.UndoAllBlocks();
        yield return waitUndo;
        yield return new WaitForSeconds(undoWaitExtra);
        undoWaitExtra = 0.0f;
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
