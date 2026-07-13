using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 15f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private Rigidbody2D rb;
    private Animator anim;
    private Player player;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        rb.gravityScale = 0f; // Ensure top-down physics (no gravity)
    }

    void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
        {
            rb.linearVelocity = Vector2.zero;
            if (anim != null) anim.SetFloat("Speed", 0f);
            return;
        }

        if (isDashing) return;

        // Get movement inputs (cross-compatible)
        float horizontal = 0f;
        float vertical = 0f;

        #if ENABLE_INPUT_SYSTEM
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) vertical = 1f;
            else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) vertical = -1f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal = 1f;
            else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) horizontal = -1f;
        }
        #else
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        #endif

        moveInput = new Vector2(horizontal, vertical).normalized;

        // Apply movement velocity
        rb.linearVelocity = moveInput * moveSpeed;

        // Flip Sprite based on movement direction
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Animator parameters
        if (anim != null)
        {
            anim.SetFloat("Speed", moveInput.magnitude);
        }

        // Dash trigger (cross-compatible)
        bool dashPressed = false;
        #if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null && kb.leftShiftKey.wasPressedThisFrame) dashPressed = true;
        #else
        if (Input.GetKeyDown(dashKey)) dashPressed = true;
        #endif

        if (dashPressed && canDash && moveInput.magnitude > 0)
        {
            if (player == null || player.HasEnoughStamina(dashStaminaCost))
            {
                StartCoroutine(PerformDash());
            }
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        if (player != null)
        {
            player.ConsumeStamina(dashStaminaCost);
        }

        // Dash in the direction of input
        rb.linearVelocity = moveInput * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
