using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static Vector2 checkpoint = new Vector2(-28.04f, -7.78f);
    private static string sceneName = "BubbleSortStory";

    private void Awake()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != sceneName)
        {
            checkpoint = new Vector2(-28.04f, -7.78f);
            sceneName = currentSceneName;
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = checkpoint;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            checkpoint = collision.transform.position;
        }
    }

}
