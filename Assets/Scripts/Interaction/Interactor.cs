using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// This goes onto whatever will be the active participant in the interaction
// basically. the player.

public class Interactor : MonoBehaviour 
{

public KeyCode interactKey;
[SerializeField] private Transform interactionPoint; 
[SerializeField] private float interactionPointRadius = 4.2f;
[SerializeField] private LayerMask interactableMask; // layer to check for collisions

[Header("For debugging")]
//[SerializeField] private InteractionPromptUI interactionPromptUI;
//private readonly Collider[] _colliders = new Collider[3];
[SerializeField] private Collider[] colliders = new Collider[3];
[SerializeField] private int _numFound;
private IInteractable interactable; // global "target" for interacting with
    
    void Start()
    {
        
    }

    void Update()
    {
        // generate Sphere of Checking - check collisions with everything on layer (interactable mask)
        _numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

        // if array not empty, get the first item in colliders & call Interact function from it
        if (_numFound > 0) {
            interactable = colliders[0].GetComponent<IInteractable>();
            if (interactable != null) {

                // found but not interacted with, show gui prompt
                // if (!interactionPromptUI.isVisible) {
                //     interactionPromptUI.SetUp(interactable.InteractionPrompt);
                // }
                interactable.ShowPrompt();

                //if (interactable != null && Keyboard.current.eKey.wasPressedThisFrame) {
                // found + keyboard pressed
                if (Input.GetKeyDown(interactKey)) {
                    Debug.Log("i have been interacted with.");
                    interactable.Interact(this); // pass self in as a parameter to interact with the thing
                }
            }
        } else { // number of things found is 0: nothing in vicinity detected
            if (interactable != null) {
                interactable.HidePrompt();
                colliders[0] = null;
                interactable = null;
                //interactionPromptUI.Close();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
