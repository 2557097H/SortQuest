using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float fallingTimer = 0f;
    private bool isFalling = false;
    public float fallingDurationThreshold = 5f;
    private bool hasDied;
    [SerializeField] private AudioSource deathSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check the current animation state of the character
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("HeroKnight_Fall"))
        {
            // Increase the falling timer if the character is falling
            fallingTimer += Time.deltaTime;

            // Check if the falling duration exceeds the threshold
            if (fallingTimer >= fallingDurationThreshold && !isFalling)
            {
                // Your code for when the character has been falling for more than 5 seconds
                Debug.Log("Character has been falling for more than 5 seconds");
                isFalling = true;  // Set a flag to prevent repeated calls
                Die();
            }
        }
        else
        {
            // Reset the falling timer when the character is not falling
            fallingTimer = 0f;
            isFalling = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Check for collision with trap objects
        if (collision2D.gameObject.CompareTag("Trap"))
        {
            Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for trigger collision with trap objects, ensuring the player has not already died
        if (other.CompareTag("Trap") && !hasDied)
        {
            hasDied = true;
            Die();
        }
    }

    private void Die()
    {
        // Disable physics simulation, trigger death animation, play death sound, and restart level
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
        deathSound.Play();
        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        // Wait for a delay before restarting the level
        yield return new WaitForSeconds(1.5f);
        RestartLevel();
    }

    private void RestartLevel()
    {
        // Reload the current scene to restart the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
