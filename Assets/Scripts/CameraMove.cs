using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{
    private const float moveSpeed = 7.5f;
    private const float cameraSpeed = 3.0f;
    
    private Vector3 moveVector = Vector3.zero;
    private float moveY = 0.0f;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Rotate the camera.
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        transform.eulerAngles += (Vector3)rotation * cameraSpeed;

        // Move the camera.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveVector = new Vector3(x, 0.0f, z) * moveSpeed;

        moveY = Input.GetAxis("Elevation");
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = transform.TransformDirection(moveVector);
        newVelocity.y += moveY * moveSpeed;
        rigidbody.velocity = newVelocity;
    }
}
