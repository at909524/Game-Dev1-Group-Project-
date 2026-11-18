
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerState currentState;

    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerCrouchState crouchState;
    public PlayerDamagedState damagedState;
    public PlayerSlideState slideState;
    public PlayerAttackState attackState;

    [Header("Components")]
    public Combat combat;
    public Damage damage;
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public Animator anim;
    public CapsuleCollider2D playerCollider;

    [Header("Movement Variables")]
    public float walkSpeed = 3.25f;
    public float runSpeed = 8;
    public float jumpForce;
    public float jumpCutMultiplier = 0.5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;

    public int facingDirection = 1;

    // Inputs
    public Vector2 moveInput;
    public bool runPressed;
    public bool jumpPressed;
    public bool jumpReleased;
    public bool attackPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Crouch Check")]
    public Transform headCheck;
    public float headCheckRadius = .2f;

    [Header("Slide Settings")]
    public float slideDuration = 0.84f;
    public float slideSpeed = 6.21f;
    public float slideStopDuration = .15f;

    public float slideHeight;
    public Vector2 slideOffset;
    public float normalHeight;
    public Vector2 normalOffset;

    private bool isSliding;

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        moveState = new PlayerMoveState(this);
        crouchState = new PlayerCrouchState(this);
        damagedState = new PlayerDamagedState(this);
        slideState = new PlayerSlideState(this);
        attackState = new PlayerAttackState(this);
    }

    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Player Rigidbody2D is missing!", this);
            return;
        }

        rb.gravityScale = normalGravity;

        ChangeState(idleState);
    }

    void Update()
    {
        if (currentState == null)
            return;

        currentState.Update();

        if (!isSliding)
            Flip();

        HandleAnimations();
    }

    void FixedUpdate()
    {
        if (currentState == null)
            return;

        currentState.FixedUpdate();

        CheckGrounded();
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == null)
        {
            Debug.LogError("Attempted to change to a null player state!", this);
            return;
        }

        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void SetColliderNormal()
    {
        if (playerCollider == null) return;

        playerCollider.size = new Vector2(
            playerCollider.size.x, normalHeight);

        playerCollider.offset = normalOffset;
    }

    public void SetColliderSlide()
    {
        if (playerCollider == null) return;

        playerCollider.size = new Vector2(
            playerCollider.size.x, slideHeight);

        playerCollider.offset = slideOffset;
    }

    public void ApplyVariableGravity()
    {
        if (rb == null) return;

        if (rb.linearVelocity.y < -0.1f)
            rb.gravityScale = fallGravity;

        else if (rb.linearVelocity.y > 0.1f)
            rb.gravityScale = jumpGravity;

        else
            rb.gravityScale = normalGravity;
    }

    void CheckGrounded()
    {
        if (groundCheck == null)
            return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    public bool CheckForCeiling()
    {
        if (headCheck == null)
            return false;

        return Physics2D.OverlapCircle(
            headCheck.position,
            headCheckRadius,
            groundLayer
        );
    }

    void HandleAnimations()
    {
        if (anim == null || rb == null)
            return;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void Flip()
    {
        if (moveInput.x > 0.1f)
            facingDirection = 1;

        else if (moveInput.x < -0.1f)
            facingDirection = -1;

        transform.localScale = new Vector3(
            facingDirection, 1, 1);
    }

    public void AttackAnimationFinished()
    {
        if (currentState != null)
            currentState.AttackAnimationFinished();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        runPressed = value.isPressed;
    }

    public void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded && !CheckForCeiling())
                jumpPressed = true;

            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                groundCheck.position, groundCheckRadius);
        }

        if (headCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(
                headCheck.position, headCheckRadius);
        }
    }
}
