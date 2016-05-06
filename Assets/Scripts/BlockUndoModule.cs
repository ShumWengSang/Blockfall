using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockUndoModule : MonoBehaviour {
    static List<BlockUndoModule> ListOfUndoModules = new List<BlockUndoModule>();

    Stack<Vector3> BlockLastPosition = new Stack<Vector3>();

    void OnDestroy()
    {
        if (ListOfUndoModules != null)
            ListOfUndoModules = null;
    }

    void OnEnable(){
        RotateGrid.OnFinishedRotating += OnFinishedRotating;
        
        ListOfUndoModules.Add(this);

        BlockLastPosition.Push(transform.position);

    }

    void OnDisable()
    {
        RotateGrid.OnFinishedRotating -= OnFinishedRotating;
        ListOfUndoModules.Remove(this);
    }

    void OnFinishedRotating()
    {
        BlockLastPosition.Push(transform.position);
    }
}
