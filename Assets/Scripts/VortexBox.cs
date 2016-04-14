﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VortexBox : MonoBehaviour {
    [AdvancedInspector.Inspect(AdvancedInspector.InspectorLevel.Debug)]
    Transform currentObject;
    Rigidbody currentRb;
    IEnumerator holdObject;
    public Vector3 collisionCenter;
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
        Vector3 size = transform.InverseTransformVector(collisionCenter);
        collider.center = new Vector3((size.x), (size.y), -1);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Name of entering item is " + other.name);
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
            yield return new WaitForFixedUpdate();
        } while (!MathHelper.IsFloatBetween(distance, 0.1f, 0f));
        SnapObject.SnapTheObject(currentObject);
        currentObject.GetComponent<Rigidbody>().isKinematic = true;
        holdObject = null;
    }

    void OnStartFalling()
    {
        CheckedItems.Clear();
        if (currentObject != null)
        {
            //1 << LayerMask.NameToLayer("Default"))
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.back, out hit, 2f ,1 << LayerMask.NameToLayer("Default"));
            if (hit.collider != null)
            {
                Debug.Log("Adding " + currentObject.name + " to checked items");
                currentObject = hit.collider.transform;
                currentObject.GetComponent<Rigidbody>().isKinematic = true;
                CheckedItems.Add(currentObject);
            }
        }
    }
}
