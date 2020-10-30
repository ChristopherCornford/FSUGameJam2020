using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class MouseLook : MonoBehaviour
{

    * Original Code Written By:
    *          Nathaniel Owens
    *          

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
*/

//The following code was written by Christopher Cornford out of necessity for network optimizing

public class MouseLook : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private Camera playerCamera = null;


    public override void OnStartAuthority()
    {
        playerCamera.gameObject.SetActive(true);

        enabled = true;
    }
}
