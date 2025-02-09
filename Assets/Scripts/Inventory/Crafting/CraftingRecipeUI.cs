using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeUI : MonoBehaviour
{
    public CraftingRecipe craftingRecipe;
    public IItemContainer itemContainer;

    // function to run when craft button pressed
    public void OnCraftButtonClick() {
        Debug.Log("Craft Button Click");
        if(craftingRecipe.CanCraft(itemContainer)){
            craftingRecipe.Craft(itemContainer);
        }
    }
}
