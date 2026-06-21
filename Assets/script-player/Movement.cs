using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public float runSpeed = 5f;
    public float stealthSpeed = 3f;

    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public bool isDead = false;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = runSpeed;
    }

    void Update()
    {
        if (isDead)
        {
            moveInput = 0f;

            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", true);

            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        bool isWalking = isMoving && Input.GetKey(KeyCode.LeftControl);
        bool isRunning = isMoving && !Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        currentSpeed = isWalking ? stealthSpeed : runSpeed;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float targetSpeed = moveInput * currentSpeed;

        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                               (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                               (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}