using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    // OnTrigger event is called when another Collider enters the trigger attached to this GameObject
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding GameObject has the name "Player"
        if (collision.gameObject.name == "Player")
        {
            // Set the colliding GameObject's parent to this GameObject (StickyPlatform)
            collision.gameObject.transform.SetParent(transform);
        }
    }

    // OnTriggerExit event is called when another Collider exits the trigger attached to this GameObject
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding GameObject has the name "Player"
        if (collision.gameObject.name == "Player")
        {
            // Remove the parent of the colliding GameObject
            collision.gameObject.transform.SetParent(null);
        }
    }
}
