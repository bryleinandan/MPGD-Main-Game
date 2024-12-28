using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickUp;

    public void PickUpItem(int id) {
        bool result = inventoryManager.AddItem(itemsToPickUp[id]);
        if (result == true) {
            Debug.Log("Item added");
        } else {
            Debug.Log("ITEM NOT ADDED");
        }
    }
    public void UseSelectedItem() {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if (receivedItem != null) {
            Debug.Log("Reveived item: " + receivedItem);
        } else {
            Debug.Log ("No item received!");
        }
    }
    public void RemoveSelectedItem() {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null) {
            Debug.Log("Removed item: " + receivedItem);
        } else {
            Debug.Log ("No item removed!");
        }
    }

}
