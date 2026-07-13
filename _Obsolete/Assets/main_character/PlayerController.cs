using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (isDashing) return;

        // Check for Dash input
        if (Input.GetKeyDown(dashKey) && canDash) {
            StartCoroutine(Dash());
            return;
        }

        // Movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        // Flip Sprite
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        // Animation Logic
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.linearVelocity = Vector2.up * jumpForce;
        }
        anim.SetBool("IsJumping", !isGrounded);

        // Crouch
        if (Input.GetKey(KeyCode.S) && isGrounded) {
            anim.SetBool("isCrouching", true);
            moveSpeed = 2f; // Slow down when crouching
        } else {
            anim.SetBool("isCrouching", false);
            moveSpeed = 5f;
        }
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;

        // Disable gravity during dash to keep it horizontal
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Determine direction to dash
        float dashDir = Input.GetAxisRaw("Horizontal");
        if (dashDir == 0) {
            dashDir = Mathf.Sign(transform.localScale.x);
        }

        // Apply dash velocity
        rb.linearVelocity = new Vector2(dashDir * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        // Restore gravity and reset dashing state
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Handle dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}