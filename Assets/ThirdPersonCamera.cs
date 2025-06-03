using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The target object (player)
    public float distance = 7f; // Distance from the target
    public float height = 2.0f; // Height of the camera above the target
    public float rotationSpeed = 2.0f; // Speed of camera rotation
    public float minVerticalAngle = -60f; // Minimum vertical angle for camera rotation
    public float maxVerticalAngle = 60f; // Maximum vertical angle for camera rotation

    private float mouseX; // Mouse X input for rotation
    private float mouseY; // Mouse Y input for rotation

    void LateUpdate()
    {
        if (target != null)
        {
            // Get mouse input for rotation
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, minVerticalAngle, maxVerticalAngle);

            // Calculate rotation based on mouse input
            Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

            // Calculate the desired position of the camera
            Vector3 negDistance = new Vector3(0.0f, height, -distance);
            Vector3 desiredPosition = target.position + rotation * negDistance;

            // Smoothly move the camera towards the desired position
            transform.position = desiredPosition;
            
            // Make the camera look at the target
            transform.LookAt(target);
        }
    }
}
