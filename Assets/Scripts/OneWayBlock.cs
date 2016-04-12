using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;
public class OneWayBlock : MonoBehaviour {
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        none
    }
    [System.Serializable]
    public class DirectionMaterialDictionary : UDictionary<Direction, Material> { }
    [Inspect]
    public DirectionMaterialDictionary directionMaterialPair;
    public Direction currentDirection;

    BoxCollider collisionBox;
    void Start()
    {
        collisionBox = GetComponent<BoxCollider>();
    }

    [Inspect, Method(MethodDisplay.Invoke)]
    void EvaluateCurrentDirection()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = directionMaterialPair[currentDirection];
    }

    void OnTriggerEnter(Collider collider)
    {
        Rigidbody rb = collider.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Direction BlockDirection = Direction.none; ;
            Vector2 vel = transform.InverseTransformDirection(rb.velocity);
            vel.Normalize();
            Debug.Log("Vel is " + vel);
            if (!MathHelper.VectorFloatIsZero(vel.x))
            {
                if (vel.x > 0.0f)
                {
                    Debug.Log("Moving right");
                    BlockDirection = Direction.Right;
                }
                else if (vel.x < 0.0f)
                {
                    Debug.Log("Moving left");
                    BlockDirection = Direction.Left;
                }
            }
            else if (!MathHelper.VectorFloatIsZero(vel.y))
            {
                if (vel.y > 0.0f)
                {
                    Debug.Log("Moving up");
                    BlockDirection = Direction.Up;
                }
                else if (vel.y < 0.0f)
                {
                    BlockDirection = Direction.Down;
                    Debug.Log("Moving down");
                }
            }
            else
            {
                Debug.LogWarning("blockdirection is null");
                return;
            }
            if(BlockDirection == currentDirection)
            {
                collisionBox.enabled = false;
            }
            else
            {
                collisionBox.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        collisionBox.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        Rigidbody rb = collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Direction BlockDirection = Direction.none; ;
            Vector2 vel = transform.InverseTransformDirection(rb.velocity);
            vel.Normalize();
            Debug.Log("Vel is " + vel);
            if (!MathHelper.VectorFloatIsZero(vel.x))
            {
                if (vel.x > 0.0f)
                {
                    Debug.Log("Moving right");
                    BlockDirection = Direction.Right;
                }
                else if (vel.x < 0.0f)
                {
                    Debug.Log("Moving left");
                    BlockDirection = Direction.Left;
                }
            }
            else if (!MathHelper.VectorFloatIsZero(vel.y))
            {
                if (vel.y > 0.0f)
                {
                    Debug.Log("Moving up");
                    BlockDirection = Direction.Up;
                }
                else if (vel.y < 0.0f)
                {
                    BlockDirection = Direction.Down;
                    Debug.Log("Moving down");
                }
            }
            else
            {
                Debug.LogWarning("blockdirection is null");
                return;
            }
            if (BlockDirection == currentDirection)
            {
                collisionBox.isTrigger = false;
            }
            else
            {
                collisionBox.isTrigger = true;
            }
        }
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
}