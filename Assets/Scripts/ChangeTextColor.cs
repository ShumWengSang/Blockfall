using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ChangeTextColor : MonoBehaviour {

    Text thisText;
    public Color endColor;
    public float duration;
	// Use this for initialization
	void Start () {
        thisText = transform.GetChild(0).GetComponent<Text>();
        thisText.DOColor(endColor, duration ).SetLoops(-1).SetEase(Ease.InOutBounce);
	}
}
