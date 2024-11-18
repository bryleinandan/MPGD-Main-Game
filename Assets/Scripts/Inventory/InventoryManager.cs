using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject mainInventoryGroup;
    public int maxStackedItems = 61;
    private InventorySlot[] inventorySlots = new InventorySlot[32];
    public GameObject inventoryItemPrefab;
    //int selectedSlot = -1;
    //select first hotbar slot as soon as game starts
    int selectedSlot = 0;

    public GameObject[] slotsByTag; // temp for getting the slots

    void Start() {
        //ChangeSelectedSlot(0);

        // Make main inventory active but invisible
        // so we always have inventory but it's not always visible
        // don't forget to set the object reference for it.
        // nvm i'm setting it via tags because the unity editor has forsaken me -J
        if (mainInventoryGroup == null) {
            GameObject[] mainInvGroupTagged = GameObject.FindGameObjectsWithTag("MainInventoryGroup");
            // there should only be one
            mainInventoryGroup = mainInvGroupTagged[0];
        }
        mainInventoryGroup.SetActive(true);

        // set scale to 0 to make it disappear.
        mainInventoryGroup.transform.localScale = Vector3.zero;

        // get all the slot objects that exist in the game: should be 32
        slotsByTag = GameObject.FindGameObjectsWithTag("InventorySlot");
        // add to inventorySlots[]. inventoryslot (24) - (31) should be the first 8
        // Debug.Log(slotsByTag[0]); // [0] gets (31)
        // Debug.Log(slotsByTag[1]); // gets (30)... perfect, we just use [0]:[7]

        // cast to InventorySlot (first 7 only)
        // (InventorySlot)collison.gameObject (bad)
        // slot = objToChange.GetComponent<NewType>() is ok if custom class inherits from monobehaviour
        // for (int i = 0; i < 8; i++) {
        //     // 7-i because using the number keys reverses order idk
		// 	inventorySlots[7-i] = slotsByTag[i].GetComponent<InventorySlot>();
		// }

        int index = 0;
        foreach (GameObject thing in slotsByTag) {
            if (index <= 7) {
                inventorySlots[7-index] = slotsByTag[index].GetComponent<InventorySlot>();
            }
            else {
                inventorySlots[index] = slotsByTag[index].GetComponent<InventorySlot>();
            }
            //Debug.Log(index);
            //Debug.Log(inventorySlots[index]);
            index++;
		}

    }

    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9) {
                ChangeSelectedSlot(number - 1);
            }
        }
        //change selected slot with scroll
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0) {
            //Debug.Log(scrollDelta > 0 ? "scroll up" : "scroll down");

            if (scrollDelta > 0) { // scroll up
                if (selectedSlot >= 7) {
                    ChangeSelectedSlot(0);
                } else {
                    ChangeSelectedSlot(selectedSlot + 1);
                }
            } else if (scrollDelta < 0) { // scroll down
                if (selectedSlot <= 0) {
                    ChangeSelectedSlot(7);
                } else {
                    ChangeSelectedSlot(selectedSlot - 1);
                }
            }
        }
    }
    
    void ChangeSelectedSlot(int newValue) {
        // Debug.Log("length of array is");
        // Debug.Log(inventorySlots.Length);
        // Debug.Log("new value is");
        // Debug.Log(newValue);
        // Debug.Log ("the object in the new slot is");
        // Debug.Log(inventorySlots[selectedSlot]);

        // Select / deselect colours
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    
    public bool AddItem(Item item) {
        //Check if any slot has same item with count lower than max
        for (int i = 0; i<inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
            itemInSlot.item == item &&
            itemInSlot.count < maxStackedItems &&
            itemInSlot.item.stackable == true) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        //Find an empty slot
        for (int i = 0; i<inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }
    
    public void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    //remove item from inventory
    public Item GetSelectedItem(bool remove) {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) { //if item is in selected slot
            Item item = itemInSlot.item;
            if (remove == true) { //if user removes(or uses) this item
                itemInSlot.count--; //remove item
                if (itemInSlot.count <= 0) { //if there was only 1 item in slot
                    Destroy(itemInSlot.gameObject); //DESTROY it
                } else { //if there was more than 1 item in slot
                    itemInSlot.RefreshCount(); //update UI to have correct number, now decreased by 1
                }
            }
            return item;
        }
        return null;
    }

    public void showMainInventory(bool tf) {
        if (tf) {
            mainInventoryGroup.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else {
            mainInventoryGroup.transform.localScale = Vector3.zero;
        }
    }

}
