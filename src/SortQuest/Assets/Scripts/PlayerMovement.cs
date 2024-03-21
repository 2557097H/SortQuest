using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private float directionX;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private LayerMask jumpableGround; // Define the ground layer for jumping

    [SerializeField] private AudioSource jumpAudioSource; // Audio source for jump sound

    private enum MovementState { idle, running, jumping, falling } // Enum for movement states

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonPressed();
        UpdateAnimationState();
    }

    // Check for button presses and update movement
    private void ButtonPressed()
    {
        directionX = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y); // Move player horizontally

        if (Input.GetButtonDown("Jump") && IsGrounded()) // Check for jump input and if player is grounded
        {
            jumpAudioSource.Play(); // Play jump sound
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce); // Apply jump force
        }

    }

    // Update player animation state based on movement and velocity
    private void UpdateAnimationState()
    {
        directionX = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        MovementState state;

        if (directionX > 0f)
        {
            state = MovementState.running; // Set state to running if moving right
            spriteRenderer.flipX = false; // Flip sprite to face right
        }
        else if (directionX < 0f)
        {
            state = MovementState.running; // Set state to running if moving left
            spriteRenderer.flipX = true; // Flip sprite to face left
        }
        else
        {
            state = MovementState.idle; // Set state to idle if not moving horizontally
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping; // Set state to jumping if velocity is positive (jumping up)
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling; // Set state to falling if velocity is negative (falling down)
        }

        animator.SetInteger("state", (int)state); // Update animator with current movement state
    }

    // Check if player is grounded
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround); // Cast a box to check for ground collision
    }
}
