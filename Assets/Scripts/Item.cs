using System.Collections;
using System.Collections.Generic;
// using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{

    [Header("Only gameplay")]
        public TileBase tile; //how the item will look like ingame (possibly not needed/different for 3d game?)
        public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4); //Range of which this tool can be used

    [Header("Only UI")]
    public bool stackable = true;
    [Header("Both")]
        public Sprite image; //how the item will look like in the inventory

}

public enum ItemType {
    BuildingBlock,
    Tool
}

public enum ActionType {
    Dig,
    Mine
}
