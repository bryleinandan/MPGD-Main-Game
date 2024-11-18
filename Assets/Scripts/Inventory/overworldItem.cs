using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class overworldItem : MonoBehaviour, IInteractable
{
    // public Mesh model;

    [Header("# Make sure these are set!")]
    public Item inventoryItem; // the Item class equivalent (will be added to inventory)
    public GameObject playerCam;
    //public TextMeshProUGUI promptText => this.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    [SerializeField] private string prompt = "Pick up!";
    public float visibilitySmoothingSpeed = 2.5f;
    public float smoothSpeed => visibilitySmoothingSpeed;
    //float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
        // "field initialiser cannot reference non static field smoothspeed" ok
    public float smooth;
    public bool autoSetLabelPosition = true;

    //[Header("Make sure there is a textmeshprogui child component.")]
    // [Header("Label position overrides:")]
    // public float labelPosX;
    // public float labelPosY;
    // public float labelPosZ;

    [Header("code does this for you / debugging")]
    public bool setToDestroy = false;
    public GameObject inventoryManagerObj;
        // for assigning in inspector because it says InventoryManager is just a GameObject
        // but we need it to be of InventoryManager class to use .addItem
    public InventoryManager inventoryManager;
    // add a setter in interactable.cs and define that here if you want to change this during runtime

    // inventoryItem.ActionType
    // inventoryItem.ItemType

    // more interface things
    //[Range(3,10)] public int interactionRange = 5; // range 
    public Camera mainCam => playerCam.GetComponent<Camera>();

    // public string interactionPromptStr {
    //     get { return interactionPromptStr; }
    //     set { interactionPromptStr = value; }
    // }

    public string interactionPromptStr { get; set; }
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
        //     mainCam = camholder.GetComponent<Camera>());
        //}
    }

    void Update() {
        ((IInteractable)this).UpdateVisibility();
        if(autoSetLabelPosition) {
            AutoSetLabelPosition();
        }

        if (setToDestroy) {
            if (transform.localScale == new Vector3(0.0f, 0.0f, 0.0f)) {
                SelfDestruct();
            }
        }
    }

    void LateUpdate() {
        //((IInteractable)this).LateUpdateLabelRotation();
        smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
        if (setToDestroy) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), smooth*2);
        }
    }

    // Handle interaction with its corresponding overworld object
    public bool Interact(Interactor interactor) {
        //Debug.Log("I have been interacted with.");

        // Add to inventory
        bool result = inventoryManager.AddItem(inventoryItem);
        if (result == true) {
            //Debug.Log("Item added");
            setToDestroy = true;
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

        // animation? poof of smoke? scaling for now
        // (implemented in update())

        Destroy(gameObject);
    }
    
       // I learned that interfaces only provide default implementation but if you generate a new methoc, it will overwrite it.
    // it's kinda difficult to access. so you can't call ShowPrompt() and expect this class to knoe
    // either new Interactable = interactable then innteractable.ShowPrompt() with all sorts of static problems
    // or (IInteractable)this.ShowPrompt()

    void AutoSetLabelPosition() {
        //var rect = promptTextMesh.GetComponent<RectTransform>();

        // Renderer meshRenderer = promptTextMesh.GetComponent<Renderer>();
        // if (meshRenderer != null) {
        //     // Calculate the top position of the mesh
        //     Vector3 topPosition = meshRenderer.bounds.max;
        //     // Set the text position above the mesh
        //     promptTextMesh.transform.position = new Vector3(topPosition.x, topPosition.y, topPosition.z);
        //     // Optionally add an offset if needed
        //     //promptTextMesh.transform.position += Vector3.up * 0.2f; // Adjust height offset as needed
        // }

        // Get Mesh filter
        MeshFilter selfMeshFilter = GetComponent<MeshFilter>();
        var model = selfMeshFilter.sharedMesh; //.mesh; / .sharedMesh
        // mesh.bounds.size * transform.localScale = actual size of mesh, but they are both vector3 so it's *scalex, *scaley... etc
        Vector3 meshSize = new Vector3((model.bounds.size.x * transform.localScale.x),
        (model.bounds.size.y * transform.localScale.y), (model.bounds.size.z * transform.localScale.z));
        
        // Use size (X, Y) to determine label margin size. see IInteractable.cs
        ((IInteractable)this).SetLabelPosition(meshSize.x, meshSize.y, meshSize.z);
        
    }

}