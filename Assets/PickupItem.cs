using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PickupItem : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public string itemDescription;

    public delegate void PickUpItemDestroyed(string pickUpItemName);
    public event PickUpItemDestroyed OnPickUpItemDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerResources.CollectItem(itemID, itemName, itemDescription);
            if (OnPickUpItemDestroyed != null) OnPickUpItemDestroyed(gameObject.name);
            Destroy(gameObject);
        }
    }
}
