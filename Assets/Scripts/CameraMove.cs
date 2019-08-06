using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	private const float moveSpeed = 7.5f;
	private const float cameraSpeed = 3.0f;

    private Vector3 inputVector = Vector3.zero;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

    private void Update()
    {
        inputVector = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);
    }

    private void FixedUpdate()
	{
        Vector3 rotation = transform.eulerAngles;
		// Rotate the camera.
		rotation += inputVector * cameraSpeed;
		transform.eulerAngles = rotation;

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		transform.Translate(move * moveSpeed * Time.deltaTime);
	}
}
