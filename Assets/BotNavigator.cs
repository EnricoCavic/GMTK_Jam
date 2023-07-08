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
    BotJumpState currentJumpState;

    public float playerSpeed;
    //public float maxSpeed = 2;
    public short sideSwitch = 1;

    public float jumpSpeed;
    //float maxJumpHeight = 2;
    //float minJumpHeight = 0.01f;




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
        var posOnFloor = transform.position - new Vector3(0, botCollider.bounds.extents.y, 0);

        RaycastHit2D hit;
        Vector2 boxCast = new Vector2(0.9f, 0.02f);

        hit = Physics2D.BoxCast(posOnFloor, boxCast, 0, Vector2.down, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            Debug.DrawRay(posOnFloor, Vector2.down * hit.distance, Color.red);
            //Debug.Log(hit.distance);

            if (hit.distance == 0) currentJumpState = BotJumpState.Jump;
            else currentJumpState = BotJumpState.DontJump;
        }

        Walk(playerSpeed);

        if (currentJumpState == BotJumpState.Jump) Jump(jumpSpeed);
    }


    public void Jump(float mJumpSpeed)
    {
        currentJumpState = BotJumpState.DontJump;

        Vector2 jump = new Vector2(0, mJumpSpeed);// * Time.deltaTime;
        rb.AddForce(jump, ForceMode2D.Impulse);
        
    }

    public void Walk(float mPlayerSpeed)
    {
        //transform.position += new Vector3(mPlayerSpeed * sideSwitch, 0f, 0f) * Time.deltaTime;

        //Vector2 walk = new Vector2(mPlayerSpeed * Time.deltaTime, 0f);
        //rb.AddForce(walk, ForceMode2D.Impulse);

        //var accelerationMultiplier = 1 - (rb.velocity.magnitude / maxSpeed);
        //Vector2 walk = new Vector2(mPlayerSpeed * accelerationMultiplier * Time.deltaTime, 0f);
        //rb.AddRelativeForce(walk * sideSwitch, ForceMode2D.Impulse);

        rb.velocity = new Vector2(mPlayerSpeed * sideSwitch, rb.velocity.y);
        


        Debug.Log(rb.velocity);
    }





}
