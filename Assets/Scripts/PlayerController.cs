using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    // player variable
    private Rigidbody _player;

    // movement variables
    private Vector2 _moveValue;
    public float speed;

    // player on ground check variables
    public LayerMask ground;
    private bool _playerGrounded;
    public float groundDrag;
    private float _playerHeight = 1.0f;

    // jump variables
    // private bool _jumpPressed;
    private float _gravity;
    // Physics.gravity = new Vector3(0, -0.5f, 0);
    private bool _readyToJump;
    public float jumpCooldown;
    public float _jumpHeight;
    public float airMultiplier;

    // Start runs before first frame update
    void Start() {

        _player = GetComponent<Rigidbody>();
        _player.freezeRotation = true;

        _readyToJump = true;
        // _jumpPressed = false;

        // gravity value
        _gravity = -0.5f;

    }

    // input captured using OnMove function
    // called by Unity automatically when 'move' action done by player
    // makes use of the input actions asset package (default movement being wasd or arrows keys)
    // value received saved as input 
    void OnMove(InputValue value) {
        _moveValue = value.Get<Vector2>();
    }

    // runs this once jump input is detected
    // void OnJump(InputValue value) {

    //     _jumpPressed = true;

    // }

    // checks every frame
    void Update() {

        // check if body is on ground - raycasting
        _playerGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.3f, ground);

        PlayerInput();

        // drag handling
        if (_playerGrounded) {
            _player.drag = groundDrag;
        } else {
            _player.drag = 0;
        }

    }

    // code to be executed every frame update
    void FixedUpdate() {

        MovePlayer();

    }

    private void PlayerInput() {

        // jump movement
        // check vertical movement - must be grounded to jump
        if(Input.GetKey("space") && _playerGrounded && _readyToJump) {

            // player jumped so no longer ready to jump
            _readyToJump = false;

            Jump();

            // player is ready to jump again
            Invoke("ResetJump", jumpCooldown);

        }

        // reset jump pressed
        // _jumpPressed = false;

    }   

    private void MovePlayer() {

        // plane movement 
        // vector formed using received input
        // player movement done by applying physics force
        // force composed of: direction (movement) * speed * Time
        // Time.fixedDeltaTime = interval in seconds at which physics and other fixed rate updates are performed
        Vector3 movement = new Vector3(_moveValue.x, 0.0f, _moveValue.y); // vector with direction

        // adjust movement based on if player is on the ground or air
        if (_playerGrounded) {
            _player.AddForce(movement * speed * 10.0f * Time.fixedDeltaTime, ForceMode.Force);    
        } else {
            _player.AddForce(movement * speed * 10.0f * Time.fixedDeltaTime * airMultiplier, ForceMode.Force);
        }

    }

    private void Jump() {

        // reset y velocity to 0 to ensure
        _player.velocity = new Vector3(_player.velocity.x, 0f, _player.velocity.z);

        // adds calculated force to player and shows visually
        // _player.AddForce(Vector3.up * _playerVelocity.y * Time.fixedDeltaTime, ForceMode.Impulse);
        _player.AddForce(transform.up * _jumpHeight, ForceMode.Impulse);

    }

    // reset jump function
    private void ResetJump() {

        _readyToJump = true;

    }

}


