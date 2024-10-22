using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public GameObject player;
    private Vector3 offset;

    // start function used to initialise offset
    void Start() {
        offset = transform.position;
    }

    // LateUpdate suitable place for follow cameras, procedural animations and gathering last known states
    // position of camera modified by adding offset of player position
    void LateUpdate() {
        transform.position = player.transform.position + offset;
    }

}
