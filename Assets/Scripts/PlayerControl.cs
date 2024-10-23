using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    
    // player variable
    private Rigidbody _player;

    // grounded player check variables
    public float _playerHeight;
    public LayerMask ground;
    private bool _grounded;

    // movement drag variables - makes movement feel smoother
    public float groundDrag;


    // jump variables
    private bool _readyToJump;
    public float jumpCooldown;
    public float jumpHeight;
    public float airMultiplier;


    // jump button key bind
    public KeyCode jumpKey = KeyCode.Space;


    // movement variables
    public float moveSpeed;

    public Transform orientation; // orientation that's synced with camera

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    private void Start() {

        _player = GetComponent<Rigidbody>();
        _player.freezeRotation = true;

        _readyToJump = true;
    }

    private void Update() {

        // player on ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.3f, ground);

        _PlayerInput();
        _SpeedControl();

        // drag handling - no drag in air to avoid weird feeling movement
        if(_grounded) {
            _player.drag = groundDrag;
        } else {
            _player.drag = 0;
        }
    }

    private void FixedUpdate() {
        _MovePlayer();
    }

    // input function - gets input from movement keys
    private void _PlayerInput() {

        // direction keys - wasd
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // jump - space
        if(Input.GetKey(jumpKey) && _readyToJump && _grounded) {

            // jump activated - no longer ready to jump
            _readyToJump = false;

            // call jump function
            _Jump();

            // continous jumping whilst holding space down - invoke reset jump function
            Invoke("_ResetJump", jumpCooldown);
        }
    }

    // move player function
    private void _MovePlayer() {

        // movement direction calculation
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // change move speed based on grounded state
        if(_grounded) {
            _player.AddForce(moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10f, ForceMode.Force);
        } else {
            _player.AddForce(moveDirection.normalized * moveSpeed * Time.fixedDeltaTime * 10f * airMultiplier, ForceMode.Force);
        }
    }

    // speed limiter function
    private void _SpeedControl() {

        // get current speed of player
        Vector3 currentVelocity = new Vector3(_player.velocity.x, 0, _player.velocity.z);

        // limit speed as needed by checking magnitude of current velocity against intended move speed
        if(currentVelocity.magnitude > moveSpeed) { 

            // calculate intended max velocity and apply to player
            Vector3 limitedVelocity = currentVelocity.normalized * moveSpeed;
            _player.velocity = new Vector3(limitedVelocity.x, 0, limitedVelocity.z);
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

}
