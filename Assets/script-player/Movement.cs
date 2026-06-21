using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // idle-run animation
    public Animator animator;

    // Movement
    public float runSpeed = 5f;
    public float stealthSpeed = 3f;

    // Jump
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Ground check
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        // bool pt running & walking animation

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


        //debug log to check if the input is being received
        //print("Move Input: " + moveInput);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        //jumping animation
        animator.SetBool("isJumping", !isGrounded);


        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Hold Left Ctrl to move slowly
        currentSpeed = isWalking ? stealthSpeed : runSpeed;

    }


    void FixedUpdate()
    {
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
