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
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
        if (collision2D.gameObject.CompareTag("Trap"))
        {
            Die();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") && !hasDied)
        {
            hasDied = true;  
            Die();
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("death");
        deathSound.Play();
        StartCoroutine(RestartAfterDelay()); 
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(1.5f); 
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 

    }
}
