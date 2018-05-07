using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResponseScript : MonoBehaviour {

    private bool Stop_Responding = false;
    private Outline outline;
    private Color originalColor;

    public Color selected_color;

    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
    }

    private void Start()
    {
        originalColor = outline.effectColor;
    }

    private void OnEnable()
    {
        EventMessenger.OnPointerDown += OnFingerDown;
        EventMessenger.OnPointerUp += OnFingerUp;
        RotateGrid.OnStartFalling += OnStartFallnig;
        RotateGrid.OnFinishedFalling += OnEndFalling;
       // EventMessenger.OnEndDragEvent += OnFingerUp;
    }

    private void OnDisable()
    {
        EventMessenger.OnPointerDown -= OnFingerDown;
        EventMessenger.OnPointerUp -= OnFingerUp;
        RotateGrid.OnStartFalling -= OnStartFallnig;
        RotateGrid.OnFinishedFalling -= OnEndFalling;
        // EventMessenger.OnEndDragEvent -= OnFingerUp;
    }

    private void OnFingerDown(EventMessenger msg)
    {
        if (Stop_Responding)
            return;
        if (msg.gameObject != this.gameObject)
            return;
        outline.effectColor = selected_color;
    }

    private void OnFingerUp(EventMessenger msg)
    {
        if (Stop_Responding)
            return;
        if (msg.gameObject != this.gameObject)
            return;
        outline.effectColor = originalColor;
    }

    private void OnStartFallnig()
    {
        Stop_Responding = true;
    }

    private void OnEndFalling()
    {
        Stop_Responding = false;
    }
}
