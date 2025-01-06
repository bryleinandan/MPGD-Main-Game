using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class OverworldItem : MonoBehaviour, IInteractable
{
    // public Mesh model;

    [Header("# Make sure inventory item is set!")]
    public Item inventoryItem; // the Item class equivalent (will be added to inventory)

    [Header("this always overrides what's in canvas")]
    [SerializeField] private string prompt = "Pick up!";
    public float visibilitySmoothingSpeed = 2.5f;
    public float smoothSpeed => visibilitySmoothingSpeed;
    //float smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
        // "field initialiser cannot reference non static field smoothspeed" ok
    public float smooth;
    public bool autoSetLabelPosition = true;

    [Header("code does this for you / debugging")]
    
    public GameObject playerCam;
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
    public Transform playerTransform { get; set; }
    public string interactionPromptStr { get; set; }
    public TextMeshPro promptTextMesh { get; set; }

    public bool promptIsVisible { get; set; }
    public Vector3 textOriginalScale { get; set; }
    public MeshFilter selfMeshFilter;

    protected virtual void Start() {
        // get own first child and get the text mesh
        promptTextMesh = this.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshPro>();
        interactionPromptStr = prompt;
        ((IInteractable)this).HidePrompt();
        textOriginalScale = promptTextMesh.GetComponent<RectTransform>().localScale;

        if (inventoryManagerObj == null) {
            // Find inventory manager!
            //inventoryManager= GameObject.FindFirstObjectByType<InventoryManager>();
            inventoryManagerObj = GameObject.Find("InventoryManager");
        }
        inventoryManager = inventoryManagerObj.GetComponent<InventoryManager>();

        // get player pos
        // if (playerCam == null) {
        //    playerCam = GameObject.Find("Orientation");
        //    playerTransform = playerCam.transform;
        // } else {
        //     playerTransform = playerCam.transform;
        // }
        // // if still can't find - just set to whatever main camera
        // if (playerCam == null) {
        //     //playerTransform = Camera.main.transform;
        //     playerCam = GameObject.Find("Player");
        // }
        // if (playerCam == null) { // i needed something who stores rotation lol
        //     playerCam = GameObject.Find("Orientation");
        //     playerTransform = playerCam.transform;
        // }

        // unity editor is not updating a reference somewhere, so I'm overwriting it
        playerCam = GameObject.Find("Orientation");
        playerTransform = playerCam.transform;
    }

    protected virtual void Update() {
        if (this == null) return;
        
        playerTransform = playerCam.transform;
        //Debug.Log(playerCam);

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
        if (this == null) return; // don't call if self is null

        ((IInteractable)this).LateUpdateLabelRotation();
        smooth = 1.0f - Mathf.Pow(0.5f, Time.deltaTime * smoothSpeed);
        if (setToDestroy) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0f, 0.0f, 0.0f), smooth*2);
        }
    }

    // Handle interaction with its corresponding overworld object
    public virtual bool Interact(Interactor interactor) {
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
        // selfDestruct();
        // this makes "return true" unreachable!
        // if we set a boolean, it'll defer executing selfDestruct() until the next frame when Update() is called.

        return true;
    }

    public virtual void SelfDestruct() { // you'll never guess what this does
        // this can overwritten in any child class by public override void selfDestruct() ...

        //Debug.Log("self destructing.");

        // animation? poof of smoke? scaling for now
        // (implemented in update())

        Destroy(gameObject);
    }

    public virtual void AutoSetLabelPosition() {
        // Get Mesh filter
        selfMeshFilter = GetComponent<MeshFilter>();
        var model = selfMeshFilter.sharedMesh; //.mesh; / .sharedMesh
        // mesh.bounds.size * transform.localScale = actual size of mesh, but they are both vector3 so it's *scalex, *scaley... etc
        Vector3 meshSize = new Vector3((model.bounds.size.x * transform.localScale.x),
        (model.bounds.size.y * transform.localScale.y), (model.bounds.size.z * transform.localScale.z));
        
        // Use size (X, Y) to determine label margin size. see IInteractable.cs
        ((IInteractable)this).SetLabelPosition(meshSize.x, meshSize.y, meshSize.z);
        
    }

    // to CLARIFY
    // Item.Spawn -> OverworldItem.Spawn
    // public void Spawn(Vector3 position, Item item) {
    //     inventoryItem = item;
    //     interactionPromptStr = item.InteractionPrompt;
    //     Instantiate(this, position, Quaternion.identity);
    // }

    public void Initialize(Item item)
    {
        // Example: Update visuals or properties
        //GetComponent<SpriteRenderer>().sprite = data.itemIcon;
        //transform.position = position;
        interactionPromptStr = item.InteractionPrompt;
    }

}