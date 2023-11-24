using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace DPA.Gameplay
{
    using Generic;
    using Managers;

    public class BotNavigator : StateMachine
    {
        public Rigidbody2D rb;
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public CapsuleCollider2D hitBox;
        [SerializeField] short sideSwitch = 1;

        public float playerSpeed;
        public float jumpForce;
        public float fallGravityMultiplier = 5f;
        [SerializeField] LayerMask collisionMask;

        [Space, SerializeField] Vector2 frontBoxSize = new Vector2(0.02f, 0.6f);
        
        Vector2 topPosition;
        ResumePauseHandler pauseHandler;

        public BotWalk botWalk;
        public BotJump botJump;
        public BotFall botFall;

        private Vector2 resumedVelocity;
        public Vector2 currentBotVelocity;

        void Awake()
        {
            botWalk = new(this);
            botJump = new(this);
            botFall = new(this);
        }

        void Start()
        {
            InitializeDefaultState(botWalk);

            pauseHandler = ResumePauseHandler.Instance;
            pauseHandler.onPaused += PauseBot;
            pauseHandler.onResumed += ResumeBot;
        }

        private void PauseBot()
        {
            resumedVelocity = rb.velocity;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.enabled = false;
        }

        private void ResumeBot()
        {
            rb.velocity = resumedVelocity;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            animator.enabled = true;
        }

        public override void Update()
        {
            if (pauseHandler.isPaused)
                return;

            currentBotVelocity = rb.velocity;
            Move(playerSpeed);
            topPosition = GetTopPosition();
            base.Update();
        }

        #region Detection

        Vector2 GetTopPosition() => transform.position + new Vector3(-sideSwitch * 0.25f, hitBox.bounds.extents.y * 2.5f, 0);
        Vector2 GetFrontPosition() => hitBox.bounds.center + new Vector3(hitBox.bounds.extents.x * sideSwitch, 0);
        public bool IsFalling => rb.velocity.y < 0.1f;

        public bool CanJumpOverWall()
        {
            var topBlockHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, 0), 1.25f, collisionMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, 0), Color.red);
            return topBlockHit.collider == null;
        }

        public bool IsGrounded()
        {
            Vector2 boxSize = new Vector2(0.6f, 0.02f);
            var groundCheckTempHit = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, Mathf.Infinity, collisionMask);
            Debug.DrawRay(transform.position, Vector2.down * groundCheckTempHit.distance, Color.red);
            if (groundCheckTempHit.collider == null) return false;

            return groundCheckTempHit.distance < 0.01f;
        }

        public bool IsNearWall()
        {  
            return Physics2D.BoxCast(GetFrontPosition(), frontBoxSize, 0, transform.right * sideSwitch, 0f, collisionMask).collider != null;
        }

        public bool IsNearHole()
        {
            var bottomHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, -2f), 2f, collisionMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, -2f), Color.red);
            return bottomHit.collider == null;
        }

        #endregion
        private void OnDrawGizmos()
        {
            Color color = Color.red;
            if (IsNearWall())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(GetFrontPosition(), frontBoxSize);
        }

        public void ApplyGravityMultiplier(float _multiplier = 1f)
        {
            rb.AddForce(fallGravityMultiplier * _multiplier * Time.deltaTime * Vector2.down, ForceMode2D.Force);
        }

        public void Jump() => Jump(jumpForce);

        public void Jump(float _jumpForce)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
            //Vector2 jump = new Vector2(0f, _jumpForce);
            //rb.AddForce(jump, ForceMode2D.Impulse);

            // StartCoroutine(LogNextFrame());
            // IEnumerator LogNextFrame()
            // {
            //     yield return null;
            //     Debug.Log("Y velocity" + rb.velocity.y);

            // }
        }

        public void Move(float _playerSpeed)
        {
            rb.velocity = new Vector2(_playerSpeed * sideSwitch, rb.velocity.y);
        }

        public void ChangeDirection()
        {
            //Debug.Log("Turn around");
            sideSwitch *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        void OnDestroy() 
        {
            pauseHandler.onPaused -= PauseBot;
            pauseHandler.onResumed -= ResumeBot;
        }
    }

}