using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private GameObject mainInventoryGroup;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        mainInventoryGroup = GameObject.Find("MainInventoryGroup");
    }

    // Update is called once per frame
    void Update()
    { 
        Debug.Log(mainInventoryGroup.transform.localScale);
        // !!make it so this only happens if MAIN MENU is closed too
        if (Input.GetMouseButtonDown(0) && mainInventoryGroup.transform.localScale == Vector3.zero) {
            // when LMB is clicked and inventory closed, remove item in selected slot
            Item removedItem = inventoryManager.GetSelectedItem(true);
            
            if (removedItem != null) {
                Debug.Log(removedItem);
            }else {
                Debug.Log("no item to remove");
            }
        }
    }
}
