using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiateBL1 : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Chest"))
        {
            LoadLevel();
        }

    }

  

    private void LoadLevel()
    {
        SceneManager.LoadScene("BubbleSortLevelOne");

    }
}
