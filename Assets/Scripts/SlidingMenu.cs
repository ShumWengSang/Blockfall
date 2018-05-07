using UnityEngine;
using System.Collections;
using AdvancedInspector;
using Lean;
using DG.Tweening;
using UnityEngine.UI;
public class SlidingMenu : MonoBehaviour {

    public bool IgnoreGUI = false;
    [Descriptor("Model Rect", "Use this rect as the model object for all the slides to follow. Usually the first slide. Set its position properly.")]
    public RectTransform DefaultRect;
     [Descriptor("Slide Parent", "Parent Transform that holds all the slides")]
    public RectTransform ContentParent;
    public RectTransform[] Content;

    public float margin;

    public float MoveTime = 1f;
    
    public Transform parentOrigParent;

    private float BoxCounter = 0;

    public Image[] transparentBoxes;
    [Inspect(InspectorLevel.Debug)]
    private float Step = 0;
    [Inspect(InspectorLevel.Debug)]
    private float InternalCounter
    {
        get { return BoxCounter; }
        set
        {
            foreach (Image box in transparentBoxes)
            {
                box.CrossFadeAlpha(0.3f, 0.5f, true);
            }
            BoxCounter = value;
            ResetBox();
        }
    }

    public bool PauseFinger
    {
        get; set;
    }

    [Inspect(InspectorLevel.Debug)]
    float []Steps;

    Sequence seq;
    // Use this for initialization
    void Start()
    {
        PauseFinger = false;
        GenerateContentParent();
       
        for(int i = 0; i < transparentBoxes.Length; i++)
        {
            transparentBoxes[i].CrossFadeAlpha(0.3f, 0.0f, true);
        }
        InternalCounter = 0;
        //seq = DOTween.Sequence();
	}

    void OnEnable()
    {
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
        LeanTouch.OnFingerSet += OnFingerSet;
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        LeanTouch.OnFingerSet -= OnFingerSet;
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
	
    public void ResetBox()
    {
        foreach (Image box in transparentBoxes)
        {
            box.CrossFadeAlpha(0.3f, 0.5f, true);
        }
        int range = Mathf.Clamp((int)BoxCounter, 0, transparentBoxes.Length - 1);
        transparentBoxes[range].CrossFadeAlpha(0.8f, 0.5f, true);
    }

    [Inspect]
    void GenerateContentParent()
    {
        float Width = DefaultRect.rect.width;
        Width *= Content.Length;
        ContentParent.rect.Set(0,0, Width, ContentParent.rect.y);

        float newWdith = DefaultRect.localPosition.x;
        for (int i = 0; i < Content.Length; i++, newWdith += DefaultRect.rect.width + margin)
        {
            Content[i].pivot = new Vector2(0f, 0.5f);
            Content[i].localPosition = new Vector2(newWdith, DefaultRect.localPosition.y);
        }
        Step = Mathf.Round(Width / Content.Length) + margin;
        Steps = new float[Content.Length];
        for(int i = 0; i < Steps.Length; i++)
        {
            Steps[i] = ContentParent.localPosition.x - i * Step;
        }
    }

    bool MoveParent = false;
    bool DontMove = false;

    Vector3 currentStepPosition;
    Vector3 OnFingerDownWorldPosition;
    Vector3 InitialContentPos;

    void OnFingerDown(LeanFinger finger)
    {
        if (PauseFinger)
            return;
        if (!IgnoreGUI || !LeanTouch.GuiInUse)
        {
            OnFingerDownWorldPosition = finger.ScreenPosition ;
            InitialContentPos = ContentParent.position;

            MoveParent = true;
            if (DOTween.IsTweening(ContentParent))
            {
                ContentParent.DOKill();
            }
        }
    }

    public int scale_movement = 5;
    void OnFingerSet(LeanFinger finger)
    {
        DontMove = false;
        if (PauseFinger)
            return;
        if (MoveParent)
        {
            float distance = OnFingerDownWorldPosition.x - finger.ScreenPosition.x;
            distance *= scale_movement;

            //We don't move  the content paret if it we are at the edges
            if ((distance > 0 && InternalCounter == 3) || (distance < 0 && InternalCounter == 0))
            {
                DontMove = true;
            }
            else
            {
                ContentParent.position = new Vector3(InitialContentPos.x - distance, ContentParent.position.y, ContentParent.position.z);
            }
        }
    }

    void OnFingerUp(LeanFinger finger)
    {
        if(DontMove)
        {
            return;
        }
        if (PauseFinger)
            return;
        if (MoveParent)
        {
            MoveParent = false;
            if (finger.GetScaledSnapshotDelta(LeanTouch.Instance.TapThreshold).magnitude <= LeanTouch.Instance.SwipeThreshold && !DOTween.IsTweening(ContentParent))
            {
                //Not a swipe.  
                ContentParent.DOLocalMoveX(Steps[(int)InternalCounter], MoveTime).SetEase(Ease.InQuad);
            }
        }
        Debug.Log("Fingerup");
    }


    void OnFingerSwipe(LeanFinger finger)
    {
        if (DontMove)
        {
            return;
        }
        if (PauseFinger)
            return;
        var swipe = finger.SwipeDelta;

        if (swipe.x < -Mathf.Abs(swipe.y))
        {
            MoveLeft();
        }

        else if (swipe.x > Mathf.Abs(swipe.y))
        {
            MoveRight();
        }
        Debug.Log("i'm swiping");
    }

    public void Button(bool back)
    {
        if(back)
        {
            //Left
            MoveLeft();
        }
        else
        {
            //Right
            MoveRight();
        }
    }

    void MoveLeft()
    {
        if (DOTween.IsTweening(ContentParent))
        {
            ContentParent.DOKill();
        }
        InternalCounter++;

        if (InternalCounter >= Content.Length)
        {
            InternalCounter--;
            if (seq == null || !seq.IsPlaying())
            {
                seq = DOTween.Sequence();
                seq.Append(ContentParent.DOLocalMoveX(-Step / 2, MoveTime).SetRelative(true));
                seq.Append(ContentParent.DOLocalMoveX(Steps[Steps.Length - 1], MoveTime)).SetAutoKill(true);
            }
        }
        else
        {
            float newPosX = Steps[(int)InternalCounter];
            ContentParent.DOLocalMoveX(newPosX, MoveTime).SetEase(Ease.InQuad);
        }

    }

    void MoveRight()
    {
        if (DOTween.IsTweening(ContentParent))
        {
            ContentParent.DOKill();
        } 
        InternalCounter--;
        if (InternalCounter < 0)
        {
            InternalCounter++;
            if (seq == null || !seq.IsPlaying())
            {
                seq = DOTween.Sequence();
                seq.Append(ContentParent.DOLocalMoveX(Step / 2, MoveTime).SetRelative(true));
                seq.Append(ContentParent.DOLocalMoveX(Steps[0], MoveTime)).SetAutoKill(true);
            }
        }
        else
        {
            float newPosX = Steps[(int)InternalCounter];
            ContentParent.DOLocalMoveX(newPosX, MoveTime).SetEase(Ease.OutQuad);
        }
    }

    public void MoveTo(int FakeinternalCounter)
    {
        if (DOTween.IsTweening(ContentParent))
        {
            ContentParent.DOKill();
        }
        float newPosX = Steps[FakeinternalCounter];
        InternalCounter = FakeinternalCounter;
        ContentParent.DOLocalMoveX(newPosX, MoveTime).SetEase(Ease.InQuad);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InternalCounter == 0)
            {
#if UNITY_ANDROID
                // Get the unity player activity
                AndroidJavaObject activity =
                   new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                   .GetStatic<AndroidJavaObject>("currentActivity");

                // call activity's boolean moveTaskToBack(boolean nonRoot) function
                // documentation:
                http://developer.android.com/reference/android/app/Activity.html#moveTaskToBack(boolean)
                activity.Call<bool>("moveTaskToBack", true);

#endif
            }
            else
            {
                MoveRight();
            }
        }
    }
}
