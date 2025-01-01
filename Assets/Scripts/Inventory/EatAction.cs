using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private GameObject mainInventoryGroup;
    private Hunger hungerScript;
    
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        mainInventoryGroup = GameObject.Find("MainInventoryGroup");
        GameObject playerHungerBar = GameObject.Find("PlayerHungerBar");
        hungerScript = playerHungerBar.GetComponent<Hunger>();
    }

    void Update()
    { 
        // Debug.Log(mainInventoryGroup.transform.localScale);
        // !!make it so this only happens if MAIN MENU is closed too (when it is eventually added)
        if (Input.GetMouseButtonDown(0) && mainInventoryGroup.transform.localScale == Vector3.zero) {
            // when LMB is clicked and inventory closed, remove item in selected slot
            Item removedItem = inventoryManager.GetSelectedItem(true);
            
            if (removedItem != null) {
                hungerScript.UpdateHunger(10);
                Debug.Log(removedItem + " removed");
            }else {
                Debug.Log("no item to remove");
            }
        }
    }
}
