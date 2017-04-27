using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
public class OneWayBlock : MonoBehaviour {
    public bool Debug_b = false;

    int BlocksPassThrough = 0;

    [System.Serializable]
    public class DirectionMaterialDictionary : UDictionary<Direction, Material> { }
    [Inspect]
    public DirectionMaterialDictionary directionMaterialPair;
    public Direction currentDirection;

    List<Collider> PassedBlocks;
    BoxCollider collisionBox;
    RaycastHit info;
    Vector2 direction = Vector2.zero;

    void OnEnable()
    {
        RotateGrid.OnFinishedRotating += OnStartFalling;
    }

    void OnDisable()
    {
        RotateGrid.OnFinishedRotating -= OnStartFalling;
    }
    void Start()
    {
        //voidLayer = LayerMask.NameToLayer("Void");
        PassedBlocks = new List<Collider>();
        collisionBox = GetComponent<BoxCollider>();
        DisableCollider(collisionBox);
        EnableCollider(collisionBox);

 
        switch (currentDirection)
        {
            case Direction.Down:
                direction = Vector2.down;
                break;
            case Direction.Left:
                direction = Vector2.left;
                break;
            case Direction.Right:
                direction = Vector2.right;
                break;
            case Direction.Up:
                direction = Vector2.up;
                break;
        }
        OnStartFalling();
    }

    [Inspect, Method(MethodDisplay.Invoke)]
    void EvaluateCurrentDirection()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = directionMaterialPair[currentDirection];
    }

    void DisableCollider(Collider col)
    {
        col.isTrigger = true;
    }

    void EnableCollider(Collider col)
    {
        col.isTrigger = false;
    }

    void OnTriggerEnter(Collider col)
    {
        Rigidbody rb = col.GetComponent<Rigidbody>();
        if (rb == null)
            return;
        if (Debug_b)
        {
            Debug.Log("Object passing through me is " + col.name);
        }
        Vector2 vel = transform.InverseTransformDirection(rb.velocity);
        Direction velDir = DetermineDirection(vel);
        if (velDir == currentDirection)
        {
            if (Debug_b)
            {
                Debug.Log("Letting object through after direction check ");
            }
            if (!PassedBlocks.Contains(col))
            {
                PassedBlocks.Add(col);
                DisableCollider(collisionBox);
                if (--BlocksPassThrough < 0)
                {
                    if (Debug_b)
                    {
                        Debug.Log("BlocksPassThrough amount is " + BlocksPassThrough);
                    }
                    EnableCollider(collisionBox);
                }
            }
        }
        else
        {
            if (Debug_b)
            {
                Debug.Log("Enabling collider as direction is wrong. The direction of col is " + velDir + " while mine is " + currentDirection + " with velocity being " + rb.velocity + " and transformed vel is " + vel);
            }
            EnableCollider(collisionBox);
        }
    }

    static Vector3[] compass = new Vector3[] { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
    Direction DetermineDirection(Vector2 vel)
    {
        var maxDot = -Mathf.Infinity;
        var ret = Vector3.zero;

        for(int i = 0; i < compass.Length; i++)
        {
            var temp = Vector3.Dot(vel, compass[i]);
            if(temp > maxDot)
            {
                ret = compass[i];
                maxDot = temp;
            }
        }
        if(ret == Vector3.left)
        {
            return Direction.Left;
        }
        else if(ret == Vector3.right)
        {
            return Direction.Right;
        }
        else if(ret == Vector3.up)
        {
            return Direction.Up;
        }
        else if(ret == Vector3.down)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.none;
        }


    }

    void OnStartFalling()
    {
        PassedBlocks.Clear();

        RaycastHit []hit = Physics.RaycastAll(transform.position, Vector2.down);
        int WoodBelowMe = 0;
        Transform wall = null;
        for(int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("WoodBlock"))
            {
                WoodBelowMe++;
            }
            else if(hit[i].collider.CompareTag("Wall"))
            {
                wall = hit[i].collider.transform;
            }
        }
        if(wall != null)
        {
            int YDifference = Mathf.RoundToInt(Mathf.Abs(transform.position.y - wall.position.y)) - 1;
            if (Debug_b)
            {
                Debug.Log("Y difference is " + YDifference);
                Debug.Log("Amount of wood below me is " + WoodBelowMe);
            }
            BlocksPassThrough = YDifference - WoodBelowMe;
            if (Debug_b)
                Debug.Log("Amount of blocks i will allow through are " + BlocksPassThrough);
            if(BlocksPassThrough > 0)
            {
                DisableCollider(collisionBox);
            }
        }
    }

    void OnDrawGizmos()
    {
        switch (currentDirection)
        {
            case Direction.Down:
                direction = Vector2.down;
                break;
            case Direction.Left:
                direction = Vector2.left;
                break;
            case Direction.Right:
                direction = Vector2.right;
                break;
            case Direction.Up:
                direction = Vector2.up;
                break;
        }
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + direction);
    }
}

public static class MathHelper
{
    static public bool FloatIsZero(float _fValue)
    {
        bool bRet = false;

        if ((_fValue < Mathf.Epsilon) && (_fValue > -Mathf.Epsilon))
        {
            bRet = true;
        }

        return bRet;
    }

    static public bool VectorFloatIsZero(float value)
    {
        bool bRet = false;

        if ((value < Vector2.kEpsilon) && (value > -Vector2.kEpsilon))
        {
            bRet = true;
        }

        return bRet;
    }

    static public bool IsFloatBetween(float value, float max, float min)
    {
        bool bRet = false;

        if ((value < max) && (value > min))
        {
            bRet = true;
        }

        return bRet;
    }
}