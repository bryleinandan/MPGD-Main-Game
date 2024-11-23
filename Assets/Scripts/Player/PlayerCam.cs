using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    
    // camera sensitivity
    public float sensX;
    public float sensY;

    // player orientation
    public Transform orientation;

    //[Header("Drop player here")]
    //public PlayerControl player;

    // camera rotation
    float xRotation;
    float yRotation;

    private void Start() {

        // lock cursor in middle of screen and hide it  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update() {

        // collect mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // camera rotation
        yRotation += mouseX;
        xRotation -= mouseY;

        // stop player from looking around further than 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate camera and player
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // correct self position
        transform.position = orientation.position;
    }

}
