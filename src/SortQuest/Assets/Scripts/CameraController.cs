using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Reference to the player. Serialized so it can be viewed in Unity Editor
    [SerializeField] private Transform player;

    private void Update()
    {
        // Set the position of the camera to follow the player on the x-axis and y-axis with an offset on the y-axis
        transform.position = new Vector3(player.position.x, player.position.y + 3, transform.position.z);
    }
}
