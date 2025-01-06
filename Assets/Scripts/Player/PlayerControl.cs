using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    
    // player variable
    private Rigidbody _player;

    // grounded player check variables
    public float playerHeight;
    public LayerMask ground;
    public bool grounded;

    // movement drag variables - makes movement feel smoother
    public float groundDrag;


    // jump variables
    private bool _readyToJump;
    public float jumpCooldown;
    public float jumpHeight;
    public float airMultiplier;

    // movement variables
    public float moveSpeed;
    public Vector2 moveValue;

    public Camera playerCam;
    private Transform cameraTransform;

    // inventoryinput script - has public variable that can be used to check inventory state
    public InventoryInput inventoryInput;

    private void Start() {

        _player = GetComponent<Rigidbody>();
        _player.freezeRotation = true;

        _readyToJump = true;

        if (playerCam != null) {
            cameraTransform = playerCam.transform;
        }
    }

    // updates dependent on machine frame rate as supposed to project
    // not as regular as fixedupdate
    private void Update() {

        // player on ground check // playerHeight * 0.6f + 0.3f
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.6f + 0.3f, ground);
        //Debug.Log("grounded = " + grounded);

        // speed limiter function
        _SpeedControl();

        // drag handling
        if(grounded) {
            _player.drag = groundDrag;
        } else {
            _player.drag = 0;
        }
    }

    // updates at regular intervals - better for physics
    private void FixedUpdate() {

        // check if inventory is open (open = vector3.one)
        // if open, stop all movement
        if(inventoryInput.inventoryOpen == false){
            //_MovePlayer();
            MovePlayerUseCamera();
        }
        
    }

    public void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    public void OnJump() {
        
        // check conditions for jump
        if(_readyToJump && grounded) {

            // jump started - no longer ready to jump
            _readyToJump = false;

            // call jump function
            _Jump();

            // code for continuous jumping here
            // player will bunny hop when they hold space down

            // reset jump function called
            _ResetJump();
        }
    }

    // move player function
    //private void _MovePlayer() {
    private void _MovePlayer(Vector3? movement_in = null) { // default value is null

        // cast move values from vector2 to vector3
        // determines direction
        //Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        // if we get nothing in, assign movement as bryle did originally
        if (movement_in != null) {
           movement = (Vector3)movement_in;
        }

        // add force
        // adjust move speed based on grounded state
        if(grounded) {
            _player.AddForce(movement * moveSpeed * Time.fixedDeltaTime * 10f, ForceMode.Force);
        } else {
            _player.AddForce(movement * moveSpeed * Time.fixedDeltaTime * 10f * airMultiplier, ForceMode.Force);
        }
    }

    // speed limiter function
    private void _SpeedControl() {

        // get current speed of player
        Vector3 currentVelocity = new Vector3(_player.velocity.x, 0f, _player.velocity.z);

        // limit speed as needed by checking magnitude of current velocity against intended move speed
        if(currentVelocity.magnitude > moveSpeed) { 

            // calculate intended max velocity and apply to player
            Vector3 limitedVelocity = currentVelocity.normalized * moveSpeed;
            _player.velocity = new Vector3(limitedVelocity.x, 0f, limitedVelocity.z);
        }
    }

    // jump function
    private void _Jump() {

        // reset y velocity - ensures consistent jump height
        _player.velocity = new Vector3(_player.velocity.x, 0f, _player.velocity.z);

        // make player jump
        // impulse - only add force once
        _player.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    // reset jump function
    private void _ResetJump() {
        _readyToJump = true;
    }

    private void MovePlayerUseCamera()
    {
        // If no camera is assigned, do nothing
        if (cameraTransform == null) return;

        // Get the forward and right directions relative to the camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Flatten the camera's forward and right vectors to ignore vertical movement
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on input Vector2
        Vector3 moveDirection = (cameraForward * moveValue.y + cameraRight * moveValue.x).normalized;

        // Call movement
        _MovePlayer(moveDirection);
    }
}
