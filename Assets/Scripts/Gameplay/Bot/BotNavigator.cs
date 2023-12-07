using UnityEngine;

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
        ResumePauseHandler pauseHandler;

        [SerializeField] short sideSwitch = 1;
        public float playerSpeed;
        public float jumpForce;
        public float fallGravityMultiplier = 5f;
        [SerializeField] LayerMask collisionMask;

        [Header("Collisions")] 
        [SerializeField] Vector2 frontBoxSize;
        [SerializeField] Vector2 groundBoxSize;
        [SerializeField] Vector2 jumpOverBoxSize;
        [SerializeField] Vector2 jumpOverOff;

        Vector2 GroundBox => new(hitBox.bounds.size.x * groundBoxSize.x, groundBoxSize.y);
        

        public BotWalk botWalk;
        public BotJump botJump;
        public BotFall botFall;

        private Vector2 resumedVelocity;

        void Awake()
        {
            botWalk = new(this);
            botJump = new(this);
            botFall = new(this);
        }

        void Start()
        {
            //groundBoxSize = new Vector2(hitBox.bounds.size.x * 0.9f, 0.02f);

            InitializeDefaultState(botWalk);

            pauseHandler = ResumePauseHandler.Instance;
            pauseHandler.onPaused += PauseBot;
            pauseHandler.onResumed += ResumeBot;
        }

        public override void FixedUpdate()
        {
            if (pauseHandler.isPaused)
                return;

            Move(playerSpeed);
            //topPosition = GetTopPosition();
            base.FixedUpdate();
        }

        #region Detection

        Vector2 topPosition => transform.position + new Vector3(-sideSwitch * hitBox.bounds.extents.x, hitBox.bounds.extents.y * 2.5f, 0);
        Vector2 frontPosition => hitBox.bounds.center + new Vector3(hitBox.bounds.extents.x * sideSwitch, 0);
        public bool IsFalling => rb.velocity.y < 0.1f;

        public Vector2 FrontBoxSize { get => FrontBoxSize1; set => FrontBoxSize1 = value; }
        public Vector2 FrontBoxSize1 { get => frontBoxSize; set => frontBoxSize = value; }

        public bool CanJumpOverWall()
        {
            //var topBlockHit = Physics2D.Raycast(TopPosition, new Vector2(sideSwitch, 0), 1f, collisionMask);
            var topBlockHit = Physics2D.BoxCast(new Vector2(jumpOverOff.x * sideSwitch, jumpOverOff.y) + topPosition, jumpOverBoxSize, 0f, Vector2.right * sideSwitch, 0f, collisionMask);
            return topBlockHit.collider == null;
        }

        public bool IsGrounded()
        {
            var groundCheckTempHit = Physics2D.BoxCast(transform.position, GroundBox, 0, Vector2.down, Mathf.Infinity, collisionMask);
            if (groundCheckTempHit.collider == null) return false;
            return groundCheckTempHit.distance < 0.02f;
        }

        public bool IsNearWall()
        {  
            return Physics2D.BoxCast(frontPosition, FrontBoxSize, 0, transform.right * sideSwitch, 0f, collisionMask).collider != null;
        }

        public bool IsNearHole()
        {
            var bottomHit = Physics2D.Raycast(topPosition, new Vector2(sideSwitch, -2f), 2f, collisionMask);
            Debug.DrawRay(topPosition, new Vector2(sideSwitch, -2f), Color.red);
            return bottomHit.collider == null;
        }

        #endregion

        #region Actions

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


        public void ApplyGravityMultiplier(float _multiplier = 1f)
        {
            rb.AddForce(fallGravityMultiplier * _multiplier * Vector2.down, ForceMode2D.Force);
        }

        public void Jump() => Jump(jumpForce);

        public void Jump(float _jumpForce)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
        }

        public void Move(float _playerSpeed)
        {
            rb.velocity = new Vector2(_playerSpeed * sideSwitch, rb.velocity.y);
        }

        public void ChangeDirection()
        {
            sideSwitch *= -1;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        #endregion

        void OnDestroy() 
        {
            pauseHandler.onPaused -= PauseBot;
            pauseHandler.onResumed -= ResumeBot;
        }

        private void OnDrawGizmos()
        {
            Color color = Color.red;
            if (IsNearWall())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(frontPosition, FrontBoxSize);

            color = Color.red;
            if (IsGrounded())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, GroundBox);

            color = Color.red;
            if(CanJumpOverWall())
                color = Color.green;

            Gizmos.color = color;
            //Gizmos.DrawLine(TopPosition, TopPosition + new Vector2(sideSwitch * 1f, 0));
            Gizmos.DrawCube(new Vector2(jumpOverOff.x * sideSwitch, jumpOverOff.y) + topPosition, jumpOverBoxSize);

        }
    }

}