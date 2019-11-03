using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

	private const float moveSpeed = 7.5f;
	private const float cameraSpeed = 3.0f;

    private Vector2 rotateInput = Vector2.zero;
    private Vector3 moveInput = Vector3.zero;

    private new Rigidbody rigidbody;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        rotateInput = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
	{
        // Rotate the camera child object.
        //Vector3 rotation = cameraTransform.eulerAngles;
		//rotation += (Vector3)(rotateInput * cameraSpeed);
        //cameraTransform.eulerAngles = rotation;

        cameraTransform.Rotate(rotateInput * cameraSpeed);
        
        // Move the camera parent object.
        rigidbody.velocity = cameraTransform.TransformDirection(moveInput * moveSpeed);
	}
}
