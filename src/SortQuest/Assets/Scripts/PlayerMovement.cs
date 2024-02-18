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

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private AudioSource jumpAudioSource;

    private enum MovementState { idle, running, jumping, falling}

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
        buttonPressed();
        UpdateAnimationState();
    }

    private void buttonPressed()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpAudioSource.Play();
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
        }

    }

    private void UpdateAnimationState()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        MovementState state;

        if (directionX > 0f )
        {
            state = MovementState.running;
            spriteRenderer.flipX = false;
        }
        else if(directionX < 0f)
        {
            state = MovementState.running;
            spriteRenderer.flipX = true;  
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int) state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


}


