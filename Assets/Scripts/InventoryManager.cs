using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 61;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    int selectedSlot = -1;
    //select first hotbar slot as soon as game starts
    void Start() {
        ChangeSelectedSlot(0);
    }
    private void Update() {
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9) {
                ChangeSelectedSlot(number - 1);
            }
        }
        //change selected slot with scroll (currently way too fast)
        if (Input.mouseScrollDelta.y >= 1) {
            Debug.Log("scroll up");
            if (selectedSlot == 7) {
                ChangeSelectedSlot(0);
            } else {
                ChangeSelectedSlot(selectedSlot+1);
            }
        } else if (Input.mouseScrollDelta.y <= -1) {
            Debug.Log("scroll down");
            if (selectedSlot == 0) {
                ChangeSelectedSlot(7);
            } else {
                ChangeSelectedSlot(selectedSlot-1);
            }
        }
    }
    void ChangeSelectedSlot(int newValue) {
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
}
