using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player object to follow
    public Vector3 offset = new Vector3(0, 5, -10); // Offset from the player
    public float smoothSpeed = 0.125f; // Smoothing speed for the camera movement

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate between the current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;

            // Make the camera look at the player
            transform.LookAt(player);
        }
    }
}