using Mirror;
using Cinemachine;
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
   public float sensitivity = 4.0f;
   [AddComponentMenu("CamHeight")]
   public class Height : MonoBehaviour
   {
   }
   void LateUpdate()
   {
       currentX -= Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;


       Vector3 Direction = new Vector3(0, 0, -distance);
       Quaternion rotation = Quaternion.Euler(CamHeight, currentX, 0);
       transform.position = lookAt.position + rotation * Direction;

       transform.LookAt(lookAt.position);
   }



   


}


//The following code was written by Christopher Cornford out of necessity for network optimizing
/*
public class MouseLook : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private CinemachineVirtualCamera playerCamera = null;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if(controls != null) { return controls; }

            return controls = new Controls();
        }
    }

    private CinemachineTransposer transposer;

    public override void OnStartAuthority()
    {
        transposer = playerCamera.GetCinemachineComponent<CinemachineTransposer>();

        playerCamera.gameObject.SetActive(true);

        enabled = true;
        
        Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    private void Look(Vector2 lookAxis)
    {
        float deltaTime = Time.deltaTime;

        transposer.m_FollowOffset.y = Mathf.Clamp(transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * deltaTime),
                                          maxFollowOffset.x,
                                          maxFollowOffset.y);

        playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
    }


}
*/