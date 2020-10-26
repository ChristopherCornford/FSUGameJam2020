using UnityEngine;
using TMPro;

public class NateRayCast : MonoBehaviour
{
    // Component thing to determine how far ahead we can pick shit up.
    [SerializeField] float PickupRange = 5.0f;
    //Component thing to make raycasting occur from our player character.
    public GameObject Body;
    //Create boolean for whether I can grab an item, as well as finding the text item.
    public bool canGrabItem;
    public TextMeshProUGUI pickUpText;

    // Update is called once per frame
    void Update()
    {
        //Generates an variable for Raycasting data to be stored in.
        RaycastHit hit;
        if (Physics.Raycast(Body.transform.position, Body.transform.forward, out hit, PickupRange))
        {
            //If the item hit by a raycast has the tag 'Item', it will update the text object with the current text and display said text.
            if (hit.transform.tag == "Item")
            {
                pickUpText.SetText("Press 'E' to pick up " + hit.transform.name + ".");
                pickUpText.enabled = true;
            }
            else
            {
                pickUpText.enabled = false;
            }
        }
        else
        {
            pickUpText.enabled = false;
        }
            
        }

}
