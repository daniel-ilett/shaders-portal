using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{
    private const float moveSpeed = 7.5f;
    private const float cameraSpeed = 3.0f;

    private Vector2 rotation = Vector2.zero;
    private Vector3 moveVector = Vector3.zero;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Rotate the camera.
        rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        transform.eulerAngles += (Vector3)rotation * cameraSpeed;

        // Move the camera.
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Elevation");
        float z = Input.GetAxis("Vertical");

        moveVector = new Vector3(x, y, z) * moveSpeed;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = transform.TransformDirection(moveVector);

    }
}
