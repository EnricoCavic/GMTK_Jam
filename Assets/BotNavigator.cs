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
    BotWalkState currentWalkState;
    BotJumpState currentJumpState = BotJumpState.DontJump;

    public float playerSpeed;
    //public float maxSpeed = 2;
    public short sideSwitch = 1;

    public float jumpSpeed;
    //float maxJumpHeight = 2;
    //float minJumpHeight = 0.01f;

    Vector2 basePosition;
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
        
        int ignoreLayer = LayerMask.NameToLayer("Bot");
        layerMask = 1 << ignoreLayer; 
        layerMask = ~layerMask;

        botCollider = this.GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        basePosition = transform.position - new Vector3(0, botCollider.bounds.extents.y, 0);
        topPosition = transform.position + new Vector3(0, botCollider.bounds.extents.y, 0);
        frontPosition = transform.position + (new Vector3(botCollider.bounds.extents.x, 0, 0) * sideSwitch);

        //if (IsGrounded()) currentJumpState = BotJumpState.Jump;
        //else currentJumpState = BotJumpState.DontJump;

        if (IsNearWall())
        {
            var topBlockHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, 0), 1.5f, layerMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, 0) * 1.5f, Color.red);
            if (topBlockHit.collider != null)
            {
                currentJumpState = BotJumpState.DontJump;
                sideSwitch *= -1;
            }
            else if(IsGrounded())
                currentJumpState = BotJumpState.Jump;
        }

        #endregion DOWNCAST


        Walk(playerSpeed);
        if (currentJumpState == BotJumpState.Jump) Jump(jumpSpeed);
    }

    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(0.9f, 0.02f);
        groundCheckTempHit = Physics2D.BoxCast(basePosition, boxSize, 0, Vector2.down, Mathf.Infinity, layerMask);
        if (groundCheckTempHit.collider == null) return false;

        //Debug.Log(hit.distance);
        Debug.DrawRay(basePosition, Vector2.down * groundCheckTempHit.distance, Color.red);
        return groundCheckTempHit.distance < 0.01f;
    }

    private bool IsNearWall()
    {
        Vector2 boxSize = new Vector2(0.02f, 0.9f);
        frontWallTempHit = Physics2D.BoxCast(frontPosition, boxSize, 0, transform.right * sideSwitch, Mathf.Infinity, layerMask);
        if (frontWallTempHit.collider == null) return false;

        return frontWallTempHit.distance < 0.3f;
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
