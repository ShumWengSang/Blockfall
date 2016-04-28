using UnityEngine;
using System.Collections;

public class BlockAnimator : MonoBehaviour {

    float distToGround;
    Animator animator;
    SpriteRenderer sr;

    Transform thisTransform;

    public BlockAnimator otherBlockAnimator;
    public bool PlayAnimator = true;

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
        if (sr != null)
        {
            distToGround = sr.sprite.bounds.extents.y;
        }
        else
        {
            distToGround = 0.5f;
        }
        animator = GetComponent<Animator>();
    }

    RaycastHit info;
	// Update is called once per frame
	void Update () {
        if (PlayAnimator)
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
            else if (info.collider.CompareTag("WoodBlock") || info.collider.CompareTag("Iron Block"))
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
        StopAllCoroutines();
        Falling = false;
    }

    void OnEndFalling()
    {
        if(PlayAnimator)
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
            if(PlayAnimator)
                animator.SetTrigger("Fall");
            StartCoroutine(fallingUpdate());
            otherBlockAnimator = null;
        }
    }
}
