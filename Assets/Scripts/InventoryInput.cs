using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    
    public GameObject[] inventory;
    public KeyCode toggleInventory;
    private bool inventoryToggled;

    private void Start() {

        // inventory is hidden by default
        inventoryToggled = false;
    }

    private void Update() {

        // when toggleInventory button pressed, show or hide inventory
        if(Input.GetKeyDown(toggleInventory)) {
            for(int i = 0; i < inventory.Length; i ++) {
                inventory[i].SetActive(!inventory[i].activeSelf);
            }
            inventoryToggled = !inventoryToggled;

            // check if inventory toggled (showing or hidden)
            // show and unlock cursor if showing, opposite if hidden
            if(inventoryToggled) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
        }
    }
}
