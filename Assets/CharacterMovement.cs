using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravityMultiplier = 3.5f;
    [SerializeField] private Transform cameraTransform; // ← Assign Main Camera in Inspector

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool resetPosition = false;
    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        startPosition = rb.position;
    }

    private void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(rb.position + Vector3.up * 0.5f, 0.5f, Vector3.down, out hit, 1.7f);

        if (Input.GetKeyDown(KeyCode.T) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            resetPosition = true;
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        Vector3 movement = moveVertical * cameraForward.normalized + moveHorizontal * cameraTransform.right;
        movement.y = 0f;
        movement = Vector3.ClampMagnitude(movement, 1f);

        Quaternion currentRotation = rb.rotation;
        Quaternion targetedRotation = currentRotation;

        if (movement != Vector3.zero)
        {
            targetedRotation = Quaternion.LookRotation(movement);
            targetedRotation = Quaternion.Slerp(currentRotation, targetedRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(targetedRotation);

            // ✅ Rotate the camera to follow the player's direction
            Quaternion cameraTargetRotation = Quaternion.Euler(0f, targetedRotation.eulerAngles.y, 0f);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // ✅ Set walking animation
        bool isWalking = movement.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        // Apply extra gravity
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Force);
        }

        // Reset position if needed
        if (resetPosition)
        {
            rb.position = startPosition;
            rb.linearVelocity = Vector3.zero;
            resetPosition = false;
        }
    }
}
