using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    private CharacterController controller;
    public Transform camera;
    private float verticalVelocity;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float turningSpeed = 8f;
    public float gravity = 9.81f;

 
    // Player's Input
    private float moveInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        GetUserInput();
        Movement();
    }

    //WASD input
    private void GetUserInput()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private float GravityForce()
    {
        // if player is grounded, it continues to remain grounded even when encountering uneven surfaces
        if(controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        // if player is not grounded then gravity takes over
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        return verticalVelocity;
    }
    private void Walking()
    {
        // Calculate movement input
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = transform.TransformDirection(move);

        //Add a conditinal here for sprinting when pressing 'shift':
        // Default walk speed
        move *= walkSpeed;

        // Applies gravity
        move.y = GravityForce();

        // Moves character
        controller.Move(move * Time.deltaTime); 
    }

    // Camera look direciton logic.
    // Turns character based on where the camera is facing
    private void PlayerLookDirection()
    {
        // Turns player in the direction that the camera is facing
        // Needs more work ( Need to mess around with configuration settings in FreeLook Camera object)
        if (Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            // Gets camera look direction
            Vector3 currentlookdirection = camera.forward;
            // Ignore y-axis direction
            currentlookdirection.y = 0;

            // Calculate player rotation based on turningSpeed
            Quaternion playerRotation = Quaternion.LookRotation(currentlookdirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * turningSpeed);
        }
    }
  
    // Movement Logic
    private void Movement()
    {
        Walking();
        PlayerLookDirection();
    }



}
