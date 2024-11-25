using System;
using System.Collections;
using System.Collections.Generic;
// using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject

{
    // Uncomment later to allow for overworldItem to be reconstructed
    // [SerializeField] private string _prompt = "Pick up!";
    // public string InteractionPrompt => _prompt;

    //public InventoryManager inventoryManager;

    [Header("Only gameplay")]
        //public TileBase tile; //how the item will look like ingame (possibly not needed/different for 3d game?)

        // please assign overworldobject prefab to this if you want to be able to re-place item later
        // [placing is currently not supported!]
        public GameObject overworldObject;
        public ItemType type;

    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4); // Range of which this tool can be used

    [Header("Only UI")]
    public bool stackable = true;
    [Header("Both")]
        public Sprite image; //how the item will look like in the inventory

}

public enum ItemType {
    Hazelnut,
    Tool
}

public enum ActionType {
    Eat,
    Attack
}
