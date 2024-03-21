using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        // Get the initial position of the background
        startpos = transform.position.x;

        // Get the length of the background sprite
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calculate the distance the camera has moved
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        // Calculate the new position based on the parallax effect
        float newPos = startpos + dist;

        // Move the background to the new position
        transform.position = new Vector3(newPos, transform.position.y, transform.position.z);

        // Check if the background needs to be repositioned
        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
