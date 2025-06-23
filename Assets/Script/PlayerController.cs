using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    private float xAxis;
    public float jumpForce = 12f;
    private float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;

    [Header("Double Jump Settings")]
    [SerializeField] private bool hasDoubleJumpAbility = false; // Khả năng double jump

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private bool hasDashAbility = false; // Khả năng dash
    [Space(5)]

    PlayerStateList pState;
    private Rigidbody2D rb;
    private Animator animator;
    private float gravity;
    private bool canDash = true;
    private bool dashed;
    private float dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pState = GetComponent<PlayerStateList>();
        gravity = rb.gravityScale;
    }

    void Update()
    {
        UpdateJumpVariables();
        GetInputs();
        if (pState.dashing) return;
        Move();
        Jump();
        UpdateAnimator();
        Flip();
        StartDash();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAxis, rb.velocity.y);
    }

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && hasDashAbility && canDash && !dashed)
        {
            // Lưu hướng dash dựa trên input hoặc hướng hiện tại
            if (xAxis != 0)
            {
                dashDirection = xAxis > 0 ? 1 : -1;
            }
            else
            {
                dashDirection = transform.localScale.x > 0 ? 1 : -1;
            }

            StartCoroutine(Dash());
            dashed = true;
        }

        // Reset dash khi chạm đất
        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;

        if (animator != null)
        {
            animator.ResetTrigger("Dashing");
            animator.SetTrigger("Dashing");
        }
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0);

        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            pState.jumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                pState.jumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            // Chỉ cho phép air jump khi đã có khả năng double jump
            else if (!Grounded() && hasDoubleJumpAbility && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            }
        }
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            coyoteTimeCounter = coyoteTime;
            pState.jumping = false;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime * 10;
        }
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    bool Grounded()
    {
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround);
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));        // Idle <-> Run
            animator.SetBool("IsGrounded", Grounded());                  // Ground check
            animator.SetFloat("VelocityY", rb.velocity.y);               // Jump / Fall
        }
    }

    // Xử lý va chạm với vật phẩm
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DoubleJump"))
        {
            hasDoubleJumpAbility = true;
            Debug.Log("Double Jump ability unlocked!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Dash"))
        {
            hasDashAbility = true;
            Debug.Log("Dash ability unlocked!");
            Destroy(other.gameObject);
        }
    }

    // Phương thức public để kiểm tra trạng thái abilities
    public bool HasDoubleJumpAbility()
    {
        return hasDoubleJumpAbility;
    }

    public bool HasDashAbility()
    {
        return hasDashAbility;
    }

    // Phương thức để reset abilities
    public void ResetDoubleJumpAbility()
    {
        hasDoubleJumpAbility = false;
    }

    public void ResetDashAbility()
    {
        hasDashAbility = false;
    }

    public void ResetAllAbilities()
    {
        hasDoubleJumpAbility = false;
        hasDashAbility = false;
    }
}