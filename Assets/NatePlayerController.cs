using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatePlayerController : MonoBehaviour
{
	CharacterController Controller;

	public float Speed;

	public Transform Cam;

	void Start()
	{
		//On Start, the game will get the controller component, lock the cursor to the center, and hide the cursor.
		Controller = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		// Grabs the value of the WASD/Arrow Key movements
		float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
		float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;

		Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
		Movement.y = 0f;

		Controller.Move(Movement);
		Quaternion CamRotation = Cam.rotation;
		CamRotation.x = 0f;
		CamRotation.z = 0f;
		transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
		if (Movement.magnitude != 0f)
		{
			transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<MouseLook>().sensivity * Time.deltaTime);

		}
	}
}