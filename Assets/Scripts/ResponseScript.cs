using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResponseScript : MonoBehaviour {
    Outline outline;

    Color originalColor;
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
       // EventMessenger.OnEndDragEvent += OnFingerUp;
    }

    private void OnDisable()
    {
        EventMessenger.OnPointerDown -= OnFingerDown;
        EventMessenger.OnPointerUp -= OnFingerUp;
        // EventMessenger.OnEndDragEvent -= OnFingerUp;
    }

    private void OnFingerDown(EventMessenger msg)
    {
        if (msg.gameObject != this.gameObject)
            return;
        outline.effectColor = selected_color;
    }

    private void OnFingerUp(EventMessenger msg)
    {
        if (msg.gameObject != this.gameObject)
            return;
        outline.effectColor = originalColor;
    }
}
