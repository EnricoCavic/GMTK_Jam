using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotWalkState
{
    Idle,
    Walk,
}

public enum BotJumpState
{
    Jump,
    DontJump
}


public class BotNavigator : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BotWalkState currentWalkState;
    BotJumpState currentJumpState = BotJumpState.DontJump;

    public float playerSpeed;
    //public float maxSpeed = 2;
    private short sideSwitch = 1;

    public float jumpSpeed;
    public float fallGravityMultiplier = 5f;
    //float maxJumpHeight = 2;
    //float minJumpHeight = 0.01f;

    Vector2 topPosition;
    Vector2 frontPosition;

    RaycastHit2D groundCheckTempHit;
    RaycastHit2D frontWallTempHit;

    Collider2D botCollider;
    Transform bottomBound;
    int layerMask;
    
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        int ignoreLayer = LayerMask.NameToLayer("Bot");
        layerMask = 1 << ignoreLayer; 
        layerMask = ~layerMask;

        botCollider = this.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        topPosition = transform.position + new Vector3(0, spriteRenderer.bounds.extents.y * 2, 0);
        frontPosition = transform.position + new Vector3(spriteRenderer.bounds.extents.x * sideSwitch, spriteRenderer.bounds.extents.y, 0);

        bool grounded = IsGrounded();
        currentJumpState = BotJumpState.DontJump;

        if (IsNearWall())
        {
            var topBlockHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, 0), 1f, layerMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, 0), Color.red);
            if (topBlockHit.collider != null)
            {
                sideSwitch *= -1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            else if(grounded)
            {
                //Debug.Log("Jump Up Obstacle");
                currentJumpState = BotJumpState.Jump;
            }
        }

        if (grounded)
        {
            animator.Play("Walk");

            var bottomHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, -1.5f), 2f, layerMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, -1.5f) * 2f, Color.red);
            if (bottomHit.collider == null)
            {
                //Debug.Log("Jump Over Hole");
                currentJumpState = BotJumpState.Jump;
            }
        }
        else
        {
            if(rb.velocity.y < -0.1f)
            {
                rb.AddForce(Vector2.down * fallGravityMultiplier);
            }
        }

        Walk(playerSpeed);
        if (currentJumpState == BotJumpState.Jump)
        {
            animator.SetTrigger("Jump");
            Jump(jumpSpeed);
        }
    }

    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(0.6f, 0.02f);
        groundCheckTempHit = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, Mathf.Infinity, layerMask);
        if (groundCheckTempHit.collider == null) return false;

        //Debug.Log(hit.distance);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckTempHit.distance, Color.red);
        return groundCheckTempHit.distance < 0.01f;
    }

    private bool IsNearWall()
    {
        Vector2 boxSize = new Vector2(0.02f, 0.6f);
        frontWallTempHit = Physics2D.BoxCast(frontPosition, boxSize, 0, transform.right * sideSwitch, Mathf.Infinity, layerMask);
        if (frontWallTempHit.collider == null) return false;

        return frontWallTempHit.distance < 0.1f;
    }

    public void Jump(float mJumpSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        currentJumpState = BotJumpState.DontJump;

        Vector2 jump = new Vector2(0, mJumpSpeed);
        rb.AddForce(jump, ForceMode2D.Impulse);
    }

    public void Walk(float mPlayerSpeed)
    {
        rb.velocity = new Vector2(mPlayerSpeed * sideSwitch, rb.velocity.y);
    }





}
