using System.Collections;
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

    public float knifeAttackDuration = 0.45f;
    public float pistolAttackDuration = 0.35f;
    public float rifleAttackDuration = 0.4f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float currentSpeed;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = runSpeed;

        UpdateWeaponAnimator();
    }

    void Update()
    {
        UpdateWeaponAnimator();

        if (isDead)
        {
            moveInput = 0f;
            ResetMovementAnimations();
            animator.SetBool("isDead", true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.J) &&
            WeaponManager.Instance.currentWeapon != 0 &&
            !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (isAttacking)
        {
            moveInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        bool isWalking = isMoving && Input.GetKey(KeyCode.LeftControl);
        bool isRunning = isMoving && !Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.localScale = new Vector3(1, 1, 1);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        currentSpeed = isWalking ? stealthSpeed : runSpeed;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
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

    void UpdateWeaponAnimator()
    {
        if (WeaponManager.Instance == null)
            return;

        animator.SetInteger("weaponType", WeaponManager.Instance.currentWeapon);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        moveInput = 0f;

        ResetMovementAnimations();

        int weapon = WeaponManager.Instance.currentWeapon;

        animator.SetInteger("weaponType", weapon);
        animator.SetBool("isAttacking", true);

        float duration = 0.4f;

        if (weapon == 1)
            duration = knifeAttackDuration;
        else if (weapon == 2)
            duration = pistolAttackDuration;
        else if (weapon == 3)
            duration = rifleAttackDuration;

        yield return new WaitForSeconds(duration);

        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void ResetMovementAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }
}