﻿using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public Portal OtherPortal;
    public GameObject lastObject
    {
        get
        {
            return theObject;
        }
        set
        {
            theObject = value;
            OtherPortal.theObject = value;
        }
    }

    private GameObject theObject;

    void OnEnable()
    {
        RotateGrid.OnUndoStart += OnStartUndo;
        RotateGrid.OnFinishedFalling += OnFinishFalling;
        RotateGrid.OnUndoFinish += OnFinishUndo;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedFalling -= OnFinishFalling;
        RotateGrid.OnUndoStart -= OnStartUndo;
        RotateGrid.OnUndoFinish -= OnFinishUndo;
    }

    bool Undoing = false;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering trigger area");
        if (lastObject != other.gameObject)
        {
            OtherPortal.lastObject = other.gameObject;
            lastObject = other.gameObject;

            other.transform.position = OtherPortal.transform.position;
            BlockUndoModule undo = other.GetComponent<BlockUndoModule>();
            undo.OnFinishedRotating();
            Debug.Log("Coming here.");
        }
        if(Undoing)
        {

            BlockUndoModule undo = other.GetComponent<BlockUndoModule>();
            RotateGrid.undoWaitExtra = 1.0f;
            undo.MoveToLastPosition();
        }
    }

    void OnStartUndo()
    {
        Undoing = true;
        lastObject = null;
    }

    void OnFinishUndo()
    {
        Undoing = false;
        lastObject = null;
    }

    void OnFinishFalling()
    {
        lastObject = null;
    }
}
