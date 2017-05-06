﻿using UnityEngine;
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

    public Transform fingerHover;
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
    [Inspect(InspectorLevel.Debug)]
    float []Steps;

    Sequence seq;
    // Use this for initialization
    void Start()
    {
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

    Vector3 currentStepPosition;
    Vector3 OnFingerDownWorldPosition;

    void OnFingerDown(LeanFinger finger)
    {
        if (!IgnoreGUI || !LeanTouch.GuiInUse)
        {
            currentStepPosition = fingerHover.position;
            OnFingerDownWorldPosition = finger.ScreenPosition ;

            MoveParent = true;
            //fingerHover.position = new Vector3(finger.GetWorldPosition(0).x, fingerHover.position.y, fingerHover.position.z);
            if (DOTween.IsTweening(ContentParent))
            {
                ContentParent.DOKill();
            }
            ContentParent.SetParent(fingerHover);
        }
    }

    public int scale_movement = 5;
    void OnFingerSet(LeanFinger finger)
    {
        if (MoveParent)
        {

            float distance = OnFingerDownWorldPosition.x - finger.ScreenPosition.x;
            distance *= scale_movement;
            fingerHover.position = new Vector3(currentStepPosition.x - distance, fingerHover.position.y, fingerHover.position.z);
            //fingerHover.position = new Vector3(finger.GetWorldPosition(0).x, fingerHover.position.y, fingerHover.position.z);
        }
    }

    void OnFingerUp(LeanFinger finger)
    {
        if (MoveParent)
        {
            ContentParent.SetParent(parentOrigParent);
            ContentParent.SetSiblingIndex(1);
            MoveParent = false;
            if (finger.GetScaledSnapshotDelta(LeanTouch.Instance.TapThreshold).magnitude <= LeanTouch.Instance.SwipeThreshold && !DOTween.IsTweening(ContentParent))
            {
                //Not a swipe.  
                ContentParent.DOLocalMoveX(Steps[(int)InternalCounter], MoveTime).SetEase(Ease.InQuad);
            }
        }
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
}
