using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overworldItem : IInteractable
{
    public Mesh model;
    public Item inventoryItem; // the inventory equivalent

    // inventoryItem.ActionType
    // inventoryItem.ItemType

    [Range(3,10)] public int interactionRange = 5; // range 

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    void Start()
    {
        // Get own model.
            // get MeshFilter component. Then get the mesh property associated with it.
        //MeshFilter selfMeshFilter = (MeshFilter)GameObject.GetComponent("MeshFilter");
        //model = selfMeshFilter.sharedMesh; //.mesh; / .sharedMesh
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(Interactor interactor) {
        Debug.Log("interaction successful.");
        return true;
    }
}
