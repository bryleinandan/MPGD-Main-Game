using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfBoundsChecker : MonoBehaviour {

    // inspiration: genshin
    // whilst grounded, hold a record of position
    // if current position -Y exceeds a certain value:
        // respawn around the recorded position

    // playercontrol has a variable, _grounded, that we can take
    [SerializeField] private bool isGrounded = true;
    private PlayerControl playerControl;
    public Vector3 lastGroundedPosition;

    public float Yboundary = -100;

    // spawn a little bit above the last gotten position
    [SerializeField] private Vector3 inAirOffset = new Vector3(0, 0.5f, 0);
    void Start() {
        playerControl = GetComponent<PlayerControl>();
        lastGroundedPosition = transform.position;
    }

    void LateUpdate() {
        isGrounded = playerControl._grounded;

        // don't softlock self by remembering a position that will always get you respawned
        if ((isGrounded) && (transform.position.y > Yboundary)) {
            lastGroundedPosition = transform.position;
        }

        // check if player surpassed boundary(/ies)
        if(transform.position.y <= Yboundary) {
            // message: you've surpassed the boundary!
            Debug.Log("so you have jumped off the world.");

            var newPosition = lastGroundedPosition + inAirOffset;
            transform.position = newPosition;
        }

    }
}
