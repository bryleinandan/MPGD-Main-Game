using System;
// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public Item Item;
    [Range(1, 99)]
    [SerializeField] public int Amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    
    public List<ItemAmount> Materials;
    public List<ItemAmount> Results;

    // checks if a certain item can be crafted
    // check if all required items are in inventory
    public bool CanCraft(IItemContainer itemContainer) {
        foreach(ItemAmount itemAmount in Materials) { 
            if (itemContainer.ItemCount(itemAmount.Item) < itemAmount.Amount){
                return false;
            }
        }
        return true;
    }

    public void Craft(IItemContainer itemContainer) {
        // check if item can be crafted
        if (CanCraft(itemContainer)) {
            // go through and remove items required for crafting
            foreach (ItemAmount itemAmount in Materials) {
                for (int i = 0; i < itemAmount.Amount; i++) {
                    itemContainer.RemoveItem(itemAmount.Item);
                }
            }
            // add crafted item to inventory
            foreach (ItemAmount itemAmount in Results) {
                for (int i = 0; i < itemAmount.Amount; i++) {
                    itemContainer.AddItem(itemAmount.Item);
                }
            }
            Debug.Log("Successful craft");
        } else {
            Debug.Log("Missing items to craft");
        }
    }
}

