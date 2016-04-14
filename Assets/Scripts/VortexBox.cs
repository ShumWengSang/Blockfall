using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VortexBox : MonoBehaviour {
    [AdvancedInspector.Inspect(AdvancedInspector.InspectorLevel.Debug)]
    Transform currentObject;
    Rigidbody currentRb;
    IEnumerator holdObject;
    public Vector3 collisionBoxWorldSize;
    BoxCollider collider;
    List<Transform> CheckedItems;
    void Start()
    {
        CheckedItems = new List<Transform>();
        collider = GetComponent<BoxCollider>();
        OnCompleteRotate();
    }

    void OnEnable()
    {
        RotateGrid.OnFinishedRotating += OnCompleteRotate;
        RotateGrid.OnStartFalling += OnStartFalling;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedRotating -= OnCompleteRotate;
        RotateGrid.OnStartFalling -= OnStartFalling;
    }

    void OnCompleteRotate()
    {
        Vector3 size = transform.InverseTransformVector(collisionBoxWorldSize);
        collider.size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), 1);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggering other object is " + other);
        if(CheckedItems.Contains(other.transform) || currentObject == other.transform)
        {
            Debug.LogWarning("Found stuff not supposed tob e here");
            return;
        }
        if (currentObject != null)
        {
            currentObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        currentRb = other.GetComponent<Rigidbody>();
        if (currentRb != null)
        {

            Debug.Log("Swapping");
            CheckedItems.Add(other.transform);
            currentObject = other.transform;
            StopAllCoroutines();
            StartCoroutine(HoldObject());

        }
    }

    IEnumerator HoldObject()
    {
        holdObject = HoldObject();
        float distance;
        do
        {
            distance = Vector2.Distance(transform.position, currentObject.transform.position);
            Debug.Log("Distance is " + distance);
            yield return new WaitForFixedUpdate();
        } while (!MathHelper.IsFloatBetween(distance, 0.1f, 0f));
        Debug.Log("I'm out");
        SnapObject.SnapTheObject(currentObject);
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        holdObject = null;
    }

    void OnStartFalling()
    {
        CheckedItems.Clear();
        if (currentObject != null)
        {
            currentObject.GetComponent<Rigidbody>().isKinematic = true;
            CheckedItems.Add(currentObject);
        }

    }
}
