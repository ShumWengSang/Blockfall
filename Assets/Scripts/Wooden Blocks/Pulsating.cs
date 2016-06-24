using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Pulsating : MonoBehaviour {

	// Use this for initialization
    Tween theTween;
    Vector3 startScale;
    void OnEnable()
    {
        startScale = transform.localScale;
        RotateGrid.OnFinishedRotating += OnStartFall;
        RotateGrid.OnFinishedFalling += OnEndFall;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedRotating -= OnStartFall;
        RotateGrid.OnFinishedFalling -= OnEndFall;
    }

	void Start () {
        theTween = transform.DOBlendableScaleBy(new Vector3(0.05f, 0.05f, 0f), 5f).SetEase(Ease.InOutElastic).SetLoops(-1, LoopType.Yoyo);
	}

    void OnEndFall()
    {
        theTween.Play();
    }

    void OnStartFall()
    {
        theTween.Pause();
        this.transform.localScale = startScale;
        SnapObject.SnapTheObject(this.transform);
    }

}
