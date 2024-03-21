using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    // Array to hold the waypoints
    [SerializeField] private GameObject[] waypoints;

    // Index of the current waypoint
    private int currentWaypointIndex = 0;

    // Speed of movement between waypoints
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        // Check if the distance between the object and the current waypoint is less than 0.1f
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;

            // If reached the end of the array, loop back to the beginning
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        // Move towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
