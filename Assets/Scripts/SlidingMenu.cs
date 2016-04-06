using UnityEngine;
using System.Collections;
using AdvancedInspector;
using Lean;
using DG.Tweening;
public class SlidingMenu : MonoBehaviour {
    [Descriptor("Model Rect", "Use this rect as the model object for all the slides to follow. Usually the first slide. Set its position properly.")]
    public RectTransform DefaultRect;
     [Descriptor("Slide Parent", "Parent Transform that holds all the slides")]
    public RectTransform ContentParent;
    public RectTransform[] Content;

    public float margin;

    public float MoveTime = 1f;

    public Transform fingerHover;
    public Transform parentOrigParent;

    [Inspect(InspectorLevel.Debug)]
    private float Step = 0;
    [Inspect(InspectorLevel.Debug)]
    private Vector3 DisplayPosition;
    [Inspect(InspectorLevel.Debug)]
    private float InternalCounter = 0;
    [Inspect(InspectorLevel.Debug)]
    float []Steps;
    WaitForEndOfFrame EndFrame;
    Sequence seq;
    // Use this for initialization
    void Start()
    {
        GenerateContentParent();
        EndFrame = new WaitForEndOfFrame();
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
        DisplayPosition = DefaultRect.localPosition;
        InternalCounter = 0;
        Steps = new float[Content.Length];
        for(int i = 0; i < Steps.Length; i++)
        {
            Steps[i] = ContentParent.localPosition.x - i * Step;
        }
    }

    void OnFingerDown(LeanFinger finger)
    {
        fingerHover.position = new Vector3(finger.GetWorldPosition(0).x, fingerHover.position.y, fingerHover.position.z);
        if (DOTween.IsTweening(ContentParent))
        {
            ContentParent.DOKill();
        }
        ContentParent.SetParent(fingerHover);
    }

    void OnFingerSet(LeanFinger finger)
    {
        fingerHover.position = new Vector3(finger.GetWorldPosition(0).x, fingerHover.position.y, fingerHover.position.z);
    }

    void OnFingerUp(LeanFinger finger)
    {
        ContentParent.SetParent(parentOrigParent);
    }


    void OnFingerSwipe(LeanFinger finger)
    {
        var swipe = finger.SwipeDelta;

        if (swipe.x < -Mathf.Abs(swipe.y))
        {
            MoveLeft();
        }

        else if (swipe.x > Mathf.Abs(swipe.y))
        {
            MoveRight();
        }
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
}
