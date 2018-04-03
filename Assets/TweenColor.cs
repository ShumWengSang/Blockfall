using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TweenColor : MonoBehaviour {
    public Color initialColor;
    public Color finalColor;

    private float duration = 2f;
    public Image leftButton;
    public Image rightButton;

    private Tween tweenLeft;
    private Tween tweenRight;

    public float seconds = 3f;

    private void Start()
    {
        StartCoroutine(waitSeconds());
    }   

    IEnumerator waitSeconds()
    {
        yield return new WaitForSeconds(seconds);
        OnStartTween();
    }

    void OnStartTween() 
    {
        tweenLeft = leftButton.DOColor(finalColor, duration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        tweenRight = rightButton.DOColor(finalColor, duration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnInputDown()
    {
        tweenLeft.Pause();
        tweenRight.Pause();
    }

    public void OnInputUp()
    {
        tweenLeft.Kill();
        tweenRight.Pause();
        leftButton.color = initialColor;
        rightButton.color = initialColor;
    }
}   
