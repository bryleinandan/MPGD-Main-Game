using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;

// using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject

{
    // Uncomment later to allow for overworldItem to be reconstructed
    [SerializeField] private string _prompt = "Pick up!";
    public string InteractionPrompt => _prompt;
    [Tooltip("how high above ground to spawn an item")]
    public float yOffset = 0.5f;

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

    // Reconstruction functions
    public virtual void Spawn(Vector3 pos, Quaternion rotation = (default), Transform parent = (default)) {  // this only spawns, doesn't delete from inventory/etc
        OverworldItem oItem = overworldObject.GetComponent<OverworldItem>();
        //oItem.Spawn(position, this); // calling from overworlditem btw

        // if (overworldObject== null) {
        //     Debug.LogError("Overworld prefab not assigned in ScriptableObject: " + itemName);
        //     return null;
        // }

        // Make a smoke
        GameObject smoke = GameObject.Find("PuffOfSmoke");
        if (smoke != null) {
            GameObject smokePuff = Instantiate(smoke, pos, rotation);
        }

        GameObject instance = Instantiate(overworldObject, pos, rotation, parent);

        // Initialize the prefab with this item's data
        OverworldItem item = instance.GetComponent<OverworldItem>();
        if (item != null) {
            item.Initialize(this);
            Debug.Log("I spawned an item at " + pos);
        }

        //return instance;
    }

}

public enum ItemType {
    BuildingBlock,
    Tool
}

public enum ActionType {
    Dig,
    Mine
}
