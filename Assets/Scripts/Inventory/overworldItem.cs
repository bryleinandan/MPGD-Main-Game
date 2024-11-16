using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class overworldItem : MonoBehaviour, IInteractable
{
    // public Mesh model;

    [Header("# Make sure these are set!")]
    public Item inventoryItem; // the Item class equivalent (will be added to inventory)
    public GameObject cameraHolder;
    //public TextMeshProUGUI promptText => this.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    [SerializeField] private string prompt = "Pick up!";

    [Header("Make sure there is a textmeshprogui child component.")]

    [Header("code does this for you / debugging")]
    public bool setToDestroy = false;
    public GameObject inventoryManagerObj;
        // for assigning in inspector because it says InventoryManager is just a GameObject
        // but we need it to be of InventoryManager class to use .addItem
    public InventoryManager inventoryManager;

    // inventoryItem.ActionType
    // inventoryItem.ItemType

    // more interface things
    //[Range(3,10)] public int interactionRange = 5; // range 
    public Camera mainCam => cameraHolder.GetComponent<Camera>();

    // public string interactionPromptStr {
    //     get { return interactionPromptStr; }
    //     set { interactionPromptStr = value; }
    // }

    public string interactionPromptStr { get; set; }
    //public TextMeshProUGUI promptTextMesh { get; set; }
    public TextMeshPro promptTextMesh { get; set; }

    public bool promptIsVisible { get; set; }
    public Vector3 textOriginalScale { get; set; }

    void Start()
    {
        // Get own model.
            // get MeshFilter component. Then get the mesh property associated with it.
        //MeshFilter selfMeshFilter = (MeshFilter)GameObject.GetComponent("MeshFilter");
        //model = selfMeshFilter.sharedMesh; //.mesh; / .sharedMesh

        // get own first child and get the text mesh
        //promptTextMesh = this.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        promptTextMesh = this.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        interactionPromptStr = prompt;
        ((IInteractable)this).HidePrompt();
        textOriginalScale = promptTextMesh.GetComponent<RectTransform>().localScale;

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

        // maincam is readonly without the setter :)
        // if (mainCam == null) {
        //     var camholder = GameObject.Find("CameraHolder");
        //     //mainCam = camholder.GetComponent<Camera>());
        // }
    }

    void Update() {
        ((IInteractable)this).UpdateVisibility();
        if (setToDestroy) {
            SelfDestruct();
        }
    }

    // Handle interaction with its corresponding overworld object
    public bool Interact(Interactor interactor) {
        //Debug.Log("I have been interacted with.");

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

    public virtual void SelfDestruct() { // you'll never guess what this does
        // this can overwritten in any child class by
        // public override void selfDestruct() ...

        Debug.Log("self destructing.");
        Destroy(gameObject);
    }

    // use default implementation in interactable
    // delete later
    // public void ShowPrompt(string setTo = "DEFAULT_") {
    //     // Default implementation as in IInteractable.cs
    //     Debug.Log("Show prompt" + setTo);
    // }

    // public void HidePrompt() {
    //     // Default implementation as in IInteractable.cs
    //     Debug.Log("Hide prompt");
    // }

    // public void UpdateVisibility() {
    //     // Default implementation as in IInteractable.cs
    // }

    // I learned that interfaces only provide default implementation but if you generate a new methoc, it will overwrite it.
    // it's kinda difficult to access. so you can't call ShowPrompt() and expect this class to knoe
    // either new Interactable = interactable then innteractable.ShowPrompt() with all sorts of static problems
    // or (IInteractable)this.ShowPrompt()
}