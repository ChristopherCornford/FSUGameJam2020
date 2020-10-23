using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

	public Transform lookAt;

	public Transform Player;

	public float distance = 10.0f;
	private float currentX = 0.0f;
	[SerializeField] float CamHeight = 15.0f;
	public float sensivity = 4.0f;
	[AddComponentMenu("CamHeight")]
	public class Height : MonoBehaviour
	{
	}
	void LateUpdate()
	{
		currentX -= Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;


		Vector3 Direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(CamHeight, currentX, 0);
		transform.position = lookAt.position + rotation * Direction;

		transform.LookAt(lookAt.position);
	}
}