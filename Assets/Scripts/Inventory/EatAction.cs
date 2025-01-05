using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private GameObject mainInventoryGroup;
    private Hunger hungerScript;
    private AudioManager audioManager;
    
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        mainInventoryGroup = GameObject.Find("MainInventoryGroup");
        GameObject playerHungerBar = GameObject.Find("PlayerHungerBar");
        hungerScript = playerHungerBar.GetComponent<Hunger>();
        GameObject AudioManager = GameObject.Find("AudioManager");
        audioManager = AudioManager.GetComponent<AudioManager>();

    }

    void Update()
    { 
        // Debug.Log(mainInventoryGroup.transform.localScale);
        // !!make it so this only happens if MAIN MENU is closed too (when it is eventually added)
        if (Input.GetKeyDown(KeyCode.F) && mainInventoryGroup.transform.localScale == Vector3.zero) {
            // when LMB is clicked and inventory closed, remove item in selected slot
            Item removedItem = inventoryManager.GetSelectedItem(true);
            
            if (removedItem != null) {
                hungerScript.UpdateHunger(10);
                audioManager.Play("FoodCrunch"); //this should play a sound when you eat..i do not hear it however
                Debug.Log(removedItem + " removed");
            }else {
                Debug.Log("no item to remove");
            }
        }
    }
}
