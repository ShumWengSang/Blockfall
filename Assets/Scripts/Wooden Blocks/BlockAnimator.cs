using UnityEngine;
using System.Collections;

public class BlockAnimator : MonoBehaviour {

    float distToGround;
    Animator animator;
    SpriteRenderer sr;

    Transform thisTransform;

    public BlockAnimator otherBlockAnimator;

    void OnEnable()
    {
        RotateGrid.OnStartFalling += OnStartFalling;
        RotateGrid.OnFinishedFalling += OnFinishFalling;
    }
    void OnDisable()
    {
        RotateGrid.OnFinishedFalling -= OnFinishFalling;
        RotateGrid.OnStartFalling -= OnStartFalling;
    }

    bool isGrounded (out RaycastHit info)
    {
        return Physics.Raycast(thisTransform.position, -Vector3.up, out info, distToGround + 1f);
    }
    bool isFalling;
    void Start()
    {
        isFalling = false;
        thisTransform = transform;
        sr = GetComponent<SpriteRenderer>();
        distToGround = sr.sprite.bounds.extents.y;
        animator = GetComponent<Animator>();
    }

    RaycastHit info;
	// Update is called once per frame
	void Update () {
        thisTransform.rotation = Quaternion.identity;
    }

    IEnumerator fallingUpdate()
    {
        while(isFalling)
        {  
            isGrounded(out info);
            
            if (info.collider == null)
            {
                OnStartFalling();
            }
            else if (info.collider.CompareTag("Wall") || info.collider.CompareTag("PlacedWallBlock"))
            {
                OnEndFalling();
            }
            else if (info.collider.CompareTag("WoodSprite"))
            {
                otherBlockAnimator = info.collider.GetComponent<BlockAnimator>();
                if (!GetNextScript().isFalling)
                {
                    OnEndFalling();
                }
                else
                {
                    OnStartFalling();
                }
            }
            else if (info.collider.CompareTag("WoodBlock"))
            {
                otherBlockAnimator = info.collider.transform.GetComponentInChildren<BlockAnimator>();
                if (!GetNextScript().isFalling)
                {
                    OnEndFalling();
                }
                else
                {
                    OnStartFalling();
                }
            }
            else
            {
                OnStartFalling();
            }
            yield return null;
        }
    }

    public BlockAnimator GetNextScript()
    {
        if(otherBlockAnimator != null)
        {
            return otherBlockAnimator.GetNextScript();
        }
        else
        {
            return this;
        }
    }

    void OnFinishFalling()
    {
        Debug.Log("Calling onfinishfalling");
        StopAllCoroutines();
        Falling = false;
    }

    void OnEndFalling()
    {
        animator.SetTrigger("Impact");
        Falling = false;
        isFalling = false;
    }

    bool Falling;
    void OnStartFalling()
    {
        if (!Falling)
        {
            Falling = true;
            isFalling = true;
            animator.SetTrigger("Fall");
            StartCoroutine(fallingUpdate());
            otherBlockAnimator = null;
        }
    }
}
