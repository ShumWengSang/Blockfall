using UnityEngine;
using System.Collections;

public class SnapObject : MonoBehaviour {
    [AdvancedInspector.Inspect, AdvancedInspector.Method(AdvancedInspector.MethodDisplay.Invoke)]
    public void SnapTheObject()
    {
        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Round(newPosition.x * 2f) * 0.5f;
        newPosition.y = Mathf.Round(newPosition.y * 2f) * 0.5f;



        if (IsThisInteger(newPosition.x))
        {
            // do stuff
            Debug.Log("Here 1");
            newPosition.x += 0.5f;
        } 
        if(IsThisInteger(newPosition.y))
        {
            Debug.Log("Here 2");
            newPosition.y += 0.5f;
            //do stuff
        }

        transform.position = newPosition;

    }
    bool IsThisInteger(float myFloat)
    {
        return Mathf.Approximately(myFloat, Mathf.RoundToInt(myFloat));
    }

}
