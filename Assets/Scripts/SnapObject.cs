﻿using UnityEngine;
using System.Collections;

public class SnapObject : MonoBehaviour {

    public static void SnapTheObject(Transform transform)
    {
        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Round(newPosition.x * 2f) * 0.5f;
        newPosition.y = Mathf.Round(newPosition.y * 2f) * 0.5f;



        if (IsThisInteger(newPosition.x))
        {
            // do stuff
            newPosition.x += 0.5f;
        }
        if (IsThisInteger(newPosition.y))
        {
            newPosition.y += 0.5f;
            //do stuff
        }

        transform.position = newPosition;
    }

    [AdvancedInspector.Inspect, AdvancedInspector.Method(AdvancedInspector.MethodDisplay.Invoke)]
    public void SnapTheObject()
    {
        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Round(newPosition.x * 2f) * 0.5f;
        newPosition.y = Mathf.Round(newPosition.y * 2f) * 0.5f;



        if (IsThisInteger(newPosition.x))
        {
            // do stuff
            newPosition.x += 0.5f;
        } 
        if(IsThisInteger(newPosition.y))
        {
            newPosition.y += 0.5f;
            //do stuff
        }

        transform.position = newPosition;

    }
    public static bool IsThisInteger(float myFloat)
    {
        return Mathf.Approximately(myFloat, Mathf.RoundToInt(myFloat));
    }

    void Start()
    {
        Destroy(this);
    }

}
