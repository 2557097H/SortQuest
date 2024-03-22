using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void RestartTheScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void toMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
