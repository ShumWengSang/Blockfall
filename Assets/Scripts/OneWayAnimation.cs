using UnityEngine;
using System.Collections;
using AdvancedInspector;

public class OneWayAnimation : MonoBehaviour {
    ///*
    Quaternion direction;

    [Inspect, Method(MethodDisplay.Invoke)]
    void EvaluateCurrentDirection()
    {
        switch (GetComponentInParent<OneWayGate>().currentDirection)
        {
            case OneWayGate.Direction.Up:
                direction = Quaternion.Euler(0, 0, 180);
                transform.rotation = direction;
                break;

            case OneWayGate.Direction.Down:
                direction = Quaternion.Euler(0, 0, 0);
                transform.rotation = direction;
                break;

            case OneWayGate.Direction.Left:
                direction = Quaternion.Euler(0, 0, 270);
                transform.rotation = direction;
                break;

            case OneWayGate.Direction.Right:
                direction = Quaternion.Euler(0, 0, 90);
                transform.rotation = direction;
                break;
        }
    }
    //*/
    // Use this for initialization
    void Start () {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        switch (GetComponentInParent<OneWayGate>().currentDirection)
        {
            case OneWayGate.Direction.Right:
                transform.Rotate(0, 0, 90, Space.World);
                //transform.rotation = direction;
                break;

            case OneWayGate.Direction.Left:
                transform.Rotate(0, 0, 270, Space.World);
                break;

            case OneWayGate.Direction.Up:
                transform.Rotate(0, 0, 180, Space.World);
                break;

            default:
            case OneWayGate.Direction.Down:
                break;
        }

        Destroy(this);
	}
}
