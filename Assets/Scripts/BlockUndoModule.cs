using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BlockUndoModule : MonoBehaviour {
    static List<BlockUndoModule> ListOfUndoModules = new List<BlockUndoModule>();
    public static float undoTime = 1.0f;
    Stack<Vector3> BlockLastPosition = new Stack<Vector3>();

    Rigidbody rb;

    bool finish = false;
    public bool MoveFinish
    {
        get
        {
            return finish;
        }
        set
        {
            finish = value;
        }
    }

    public static void UndoAllBlocks()
    {
        for(int i = 0; i < ListOfUndoModules.Count; i++)
        {
            ListOfUndoModules[i].MoveToLastPosition();
        }
    }
    void OnDestroy()
    {
        if (ListOfUndoModules != null)
            ListOfUndoModules = null;
    }

    void OnEnable(){
        RotateGrid.OnFinishedRotating += OnFinishedRotating;
        
        ListOfUndoModules.Add(this);

        BlockLastPosition.Push(transform.position);

        rb = GetComponent<Rigidbody>();

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

    void MoveToLastPosition(bool changeKinematics = false)
    {
        if (changeKinematics)
            rb.isKinematic = true;
        GetComponent<Rigidbody>().DOMove(BlockLastPosition.Pop(), undoTime).SetEase(Ease.OutQuad);
    }

    IEnumerator MoveObjectTo(Vector3 original, Vector3 dest, float time, Transform theObject)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            theObject.position = Vector3.Lerp(original, dest, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        finish = true;
    }
}
