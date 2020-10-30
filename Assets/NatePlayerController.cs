using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Original Code written by Nathaniel Owens


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
		Movement.y = 0.0f;

		Controller.Move(Movement);
		Quaternion CamRotation = Cam.rotation;
		CamRotation.x = 0f;
		CamRotation.z = 0f;
		transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
		if (Movement.magnitude != 0f)
		{
			transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<MouseLook>().sensitivity * Time.deltaTime);

		}
	}
}


/*
public class NatePlayerController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private CharacterController controller = null;

    private Vector2 previousInput;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }

            return controls = new Controls();
        }
    }


    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());

        Controls.Player.Move.canceled += ctx => ResetMovement();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [ClientCallback]
    private void Update() => Move();

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void Move()
    {
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;

        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * movementSpeed * Time.deltaTime);
    }
}
*/