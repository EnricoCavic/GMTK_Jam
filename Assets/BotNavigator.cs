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
    public short sideSwitch = 1;

    public float jumpSpeed;
    float maxJumpHeight = 2;
    float minJumpHeight = 0.01f;




    Collider2D botCollider;
    Transform bottomBound;
    int layerMask;

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        
        int ignoreLayer = LayerMask.NameToLayer("Bot");
        layerMask = 1 << ignoreLayer; 
        layerMask = ~layerMask;

        botCollider = this.GetComponent<Collider2D>();
        


    }

    // Update is called once per frame
    void Update()
    {
        Walk(playerSpeed);
        Jump(jumpSpeed);


        var posOnFloor = transform.position - new Vector3(0, botCollider.bounds.extents.y, 0);

        RaycastHit2D hit;
        hit = Physics2D.Raycast(posOnFloor, Vector2.down, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            Debug.DrawRay(posOnFloor, Vector2.down * hit.distance, Color.red);
            Debug.Log(hit.distance);
            
            if(hit.distance > maxJumpHeight)
            {
                jumpSpeed = 0;
            }
        }

    }


    public void Jump(float mJumpSpeed)
    {
        Vector2 jump = new Vector2(0, mJumpSpeed) * Time.deltaTime;
        rb.AddForce(jump, ForceMode2D.Impulse);
    }

    public void Walk(float mPlayerSpeed)
    {
        transform.position += new Vector3(mPlayerSpeed * sideSwitch, 0f, 0f) * Time.deltaTime;
    }





}
