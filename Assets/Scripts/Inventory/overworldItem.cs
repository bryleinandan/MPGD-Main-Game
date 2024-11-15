using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class overworldItem : MonoBehaviour, IInteractable
{
    // public Mesh model;

    [Header("# Make sure these are set!")]
    public Item inventoryItem; // the Item class equivalent (will be added to inventory)
    public GameObject cameraHolder;
    public TextMeshProUGUI promptText => this.GetComponent<TextMeshProUGUI>();
    [SerializeField] private string prompt = "Pick up!";

    [Header("Make sure there is a textmeshprogui component.")]

    [Header("code does this for you / debugging")]
    public InventoryManager inventoryManager;
    //public GameObject inventoryManager;
    public GameObject inventoryManagerObj;
        // for assigning in inspector because it says InventoryManager is just a GameObject
        // but we need it to be of InventoryManager class to use .addItem

    //// ^ this object is designed to be used in conjunction with item.cs, so getters are:
    // inventoryItem.ActionType
    // inventoryItem.ItemType

    // Interface things
    //[Range(3,10)] public int interactionRange = 5; // range 
    public string InteractionPrompt => prompt;
    public Camera mainCam => cameraHolder.GetComponent<Camera>();
    // textmeshpro initialisation, prompt txt

    public bool setToDestroy = false;
    public bool promptIsVisible = false;

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

        if (mainCam == null) {
            var camholder = GameObject.Find("CameraHolder");
            // ok and now I can't overwrite mainCam because it's read only
        }
    }

    void Update() {
        if (setToDestroy) {
            SelfDestruct();
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

    public void ShowPrompt(string promptText) {
        promptText = prompt;
        promptIsVisible = true;
    }

    public void HidePrompt() {
        promptIsVisible = false;
    }


    public virtual void SelfDestruct() { // you'll never guess what this does
        // this can overwritten in any child class by
        // public override void selfDestruct() ...

        Debug.Log("self destructing.");
        Destroy(gameObject);
    }

}
