using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// This goes onto whatever will be the active participant in the interaction
// basically. the player.

public class Interactor : MonoBehaviour 
{
[SerializeField] private Transform _interactionPoint; 
[SerializeField] private float _interactionPointRadius = 1.2f;
[SerializeField] private LayerMask _interactableMask;

//private readonly Collider[] _colliders = new Collider[3];
[SerializeField] private Collider[] _colliders = new Collider[3];
[SerializeField] private int _numFound;
    
    void Start()
    {
        
    }

    void Update()
    {
        // generate Sphere of Checking - check collisions with everything on layer (interactable mask)
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        // if array not empty, get the first item in colliders & call Interact function from it
        if (_numFound > 0) {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if (interactable != null && Keyboard.current.eKey.wasPressedThisFrame) {
                interactable.Interact(this); // pass self in as a parameter
            }
        } else {
            _colliders[0] = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
