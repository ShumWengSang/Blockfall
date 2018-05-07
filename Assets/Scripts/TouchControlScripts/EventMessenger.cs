using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EventMessenger : MonoBehaviour {
    public RotateGrid grid;

    public delegate void Message(EventMessenger obj);
    public static event Message OnStartDragEvent;
    public static event Message OnEndDragEvent;
    public static event Message OnPointerDown;
    public static event Message OnPointerUp;

    private EventTrigger trigger;

    private bool TriggerRespondFlag = true;

    private void OnEnable()
    {
        OnEndDragEvent += OnEndDragEvent_Response;
        OnStartDragEvent += OnStartDragEvent_Response;
        RotateGrid.OnStartFalling += OnStartFalling;
        RotateGrid.OnFinishedFalling += OnEndFalling;
    }

    private void OnDisable()
    {
        OnEndDragEvent -= OnEndDragEvent_Response;
        OnStartDragEvent -= OnStartDragEvent_Response;
    }

    private void Awake()
    {
        trigger = GetComponent<EventTrigger>();
    }
    // Use this for initialization
    void Start()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => { OnStartDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnFingerDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointDown((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnPointUp((PointerEventData)data); });
        trigger.triggers.Add(entry);

        TriggerRespondFlag = true;
        
    }

    #region TriggerEventMessengers
    public void OnFingerDrag(PointerEventData data)
    {
        if (TriggerRespondFlag)
        {
            grid.OnFingerDrag(data);
            OnStartDragEvent(this);
        }
    }

    public void OnStartDrag(PointerEventData data)
    {
        if (TriggerRespondFlag)
        {
            grid.OnStartDrag(data);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (TriggerRespondFlag)
        {
            grid.OnEndDrag(data);
            OnEndDragEvent(this);
        }
    }

    public void OnPointDown(PointerEventData data)
    {
        if (TriggerRespondFlag)
        {
            if (OnPointerDown != null) OnPointerDown(this);
            grid.AnimateDrag_PointDown(data);
        }
    }

    public void OnPointUp(PointerEventData data)
    {
        if (OnPointerUp != null) OnPointerUp(this);
    }

    #endregion

    private void OnStartDragEvent_Response(EventMessenger obj)
    {
        if(obj != this)
        {
            TriggerRespondFlag = false;
        }
    }

    private void OnEndDragEvent_Response(EventMessenger obj)
    {
        TriggerRespondFlag = true;
    }

    private void OnStartFalling()
    {
        trigger.enabled = false;
    }

    private void OnEndFalling()
    {
        trigger.enabled = true;
    }
}
