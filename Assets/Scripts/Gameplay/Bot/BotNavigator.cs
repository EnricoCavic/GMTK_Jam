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
        [Space, SerializeField] float holeCheckDirection;
        [SerializeField] float holeCheckDistance;
        [Space, SerializeField] Vector2 jumpOverBoxSize;
        [SerializeField] Vector2 jumpOverPosition;

        Vector2 GroundBox => new(hitBox.bounds.size.x * groundBoxSize.x, groundBoxSize.y);
        Vector2 HoleCheckDir => new(sideSwitch, holeCheckDirection);

        public BotWalk botWalk;
        public BotJump botJump;
        public BotFall botFall;

        Vector2 resumedVelocity;
        Vector2 currentFramteTopPosition;

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

        public override void FixedUpdate()
        {
            if (pauseHandler.isPaused)
                return;

            currentFramteTopPosition = GetTopPosition();
            Move(playerSpeed);
            base.FixedUpdate();
        }

        #region Detection

        Vector2 GetTopPosition() => transform.position + new Vector3(0f, hitBox.bounds.extents.y * 2.5f);
        Vector2 GetFrontPosition() => hitBox.bounds.center + new Vector3(hitBox.bounds.extents.x * sideSwitch, 0);
        public bool IsFalling => rb.velocity.y < 0.1f;

        public bool CanJumpOverWall()
        {
            return Physics2D.BoxCast(
                new Vector2(jumpOverPosition.x * sideSwitch, jumpOverPosition.y) + currentFramteTopPosition,
                jumpOverBoxSize,
                0f,
                Vector2.right * sideSwitch,
                0f,
                collisionMask
                ).collider == null;
        }

        public bool IsGrounded()
        {
            var groundCheckTempHit = Physics2D.BoxCast(transform.position, GroundBox, 0, Vector2.down, Mathf.Infinity, collisionMask);
            if (groundCheckTempHit.collider == null) return false;
            return groundCheckTempHit.distance < 0.02f;
        }

        public bool IsNearWall()
        {  
            return Physics2D.BoxCast(GetFrontPosition(), frontBoxSize, 0f, transform.right * sideSwitch, 0f, collisionMask).collider != null;
        }

        public bool IsNearHole()
        {
            return Physics2D.Raycast(currentFramteTopPosition, HoleCheckDir.normalized, holeCheckDistance, collisionMask).collider == null;
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
            var topPosition = GetTopPosition();

            Color color = Color.red;
            if (IsNearWall())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(GetFrontPosition(), frontBoxSize);

            color = Color.red;
            if (IsGrounded())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, GroundBox);

            color = Color.red;
            if(CanJumpOverWall())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawCube(new Vector2(jumpOverPosition.x * sideSwitch, jumpOverPosition.y) + topPosition, jumpOverBoxSize);

            color = Color.red;
            if(IsNearHole())
                color = Color.green;

            Gizmos.color = color;
            Gizmos.DrawLine(topPosition, HoleCheckDir.normalized * holeCheckDistance + topPosition);

        }
    }

}