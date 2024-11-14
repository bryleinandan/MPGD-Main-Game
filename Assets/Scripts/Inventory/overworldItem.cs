using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class overworldItem : MonoBehaviour, IInteractable
{
    // public Mesh model;

    [Header("Mandatory")]
    public Item inventoryItem; // the Item class equivalent (will be added to inventory)

    [Header("Set second one if you can")]
    public InventoryManager inventoryManager;
    //public GameObject inventoryManager;
    public GameObject inventoryManagerObj;
        // for assigning in inspector because it says InventoryManager is just a GameObject
        // but we need it to be of InventoryManager class to use .addItem

    //// ^ this object is designed to be used in conjunction with item.cs, so getters are:
    // inventoryItem.ActionType
    // inventoryItem.ItemType

    //[Range(3,10)] public int interactionRange = 5; // range 
    [SerializeField] private string _prompt = "Pick up!";
    public string InteractionPrompt => _prompt;

    public bool setToDestroy = false;

    void Start()
    {
        // Get own model.
            // get MeshFilter component. Then get the mesh property associated with it.
        //MeshFilter selfMeshFilter = (MeshFilter)GameObject.GetComponent("MeshFilter");
        //model = selfMeshFilter.sharedMesh; //.mesh; / .sharedMesh

        if (inventoryManagerObj == null) {
            // Find inventory manager!
            //inventoryManager= GameObject.FindFirstObjectByType<InventoryManager>();
            inventoryManagerObj = GameObject.Find("InventoryManager");
        }
        inventoryManager = inventoryManagerObj.GetComponent<InventoryManager>();

        // if inventoryManager is STILL null
        // if (inventoryManager == null) {
        //     inventoryManager= GameObject.FindFirstObjectByType<InventoryManager>();
        // }
    }

    void Update() {
        if (setToDestroy) {
            selfDestruct();
        }
    }

    // Handle interaction with its corresponding overworld object
    public bool Interact(Interactor interactor) {
        Debug.Log("I have been interacted with.");

        // Add to inventory
        bool result = inventoryManager.AddItem(inventoryItem);
        if (result == true) {
            //Debug.Log("Item added");
        } else {
            Debug.Log("ITEM NOT ADDED");
        }

        // Remove the instanced object from overworld
        //selfDestruct();
        // this makes "return true" unreachable!
        // if we set a boolean, it'll defer executing selfDestruct() until the next frame when Update() is called.

        return true;
    }

    public virtual void selfDestruct() { // you'll never guess what this does
        // this can overwritten in any child class by
        // public override void selfDestruct() ...

        Debug.Log("self destructing.");
        Destroy(gameObject);
    }

}
