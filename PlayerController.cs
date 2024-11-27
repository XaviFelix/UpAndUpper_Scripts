/***************************************************************
*file: EnemyAttack.cs
*author: Xavier Felix & Darren Banhthai
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/23/24
*
*purpose: To control player behavior such as walking, running,
*         and jumping based on player input and environmental cues
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    private CharacterController controller;
    public new Transform camera;
    private float verticalVelocity;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float turningSpeed = 8f;
    public float gravity = 9.81f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 2f;

    // Player's Input
    private float moveInput;
    private float turnInput;
    private bool isSprinting;
    private bool isJumping;

    // Push data
    private bool isBeingPushed = false;
    private Vector3 pushDirection;
    private Vector3 pushVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isBeingPushed = false;
        pushVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
        Movement();

        // pushback when moving
        if (isBeingPushed)
        {
            controller.Move(pushVelocity * Time.deltaTime);
        }
    }

    // function: GetUserInput
    // purpose: record WASD, shift and space from user input
    private void GetUserInput()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isJumping = Input.GetKeyDown(KeyCode.Space) && controller.isGrounded;
    }

    // function: GravityForce
    // purpose: Used to determine player's vertical velocity 
    //          dependent on jumping and grounded status.
    //          returns vertical velocity calculation (effects of gravity)
    private float GravityForce()
    {
        // if player is grounded, it continues to remain grounded even when encountering uneven surfaces
        if (controller.isGrounded && !isJumping)
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

    // function: CalcVerticalForce
    // purpose: Used to determine player's vertical velocity 
    //          dependent on user's input via spacebar (jump button)
    //          returns vertical velocity calculation
    private float CalcVerticalForce()
    {
        // calculate vertical jump velocity  
        if (isJumping)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity); 
        }
        // calculate vertical velocity due to effects of only gravity
        else
        {
            verticalVelocity = GravityForce();
        }
        return verticalVelocity;
    }

    // function: Walking
    // purpose: Allows player to walk based on user's input (WASD)
    private void Walking()
    {
        // calculate movement input
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = transform.TransformDirection(move);

        // adjust speed whether sprinting or walking
        move *= isSprinting ? sprintSpeed : walkSpeed;

        // applies vertical force
        move.y = CalcVerticalForce();

        // moves character
        controller.Move(move * Time.deltaTime);
    }


    // function: PlayerLookDirection
    // purpose: Turns character based on where the camera is facing
    private void PlayerLookDirection()
    {
        // Turns player in the direction that the camera is facing
        if (Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            // Gets camera look direction
            Vector3 currentlookdirection = camera.forward;
            // Ignores y-axis direction
            currentlookdirection.y = 0;

            // Calculate player rotation based on turningSpeed
            Quaternion playerRotation = Quaternion.LookRotation(currentlookdirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * turningSpeed);
        }
    }

    // function: Movement
    // purpose: Calls both Walking and PlayerLookDireciton methods
    private void Movement()
    {
        Walking();
        PlayerLookDirection();
    }

    // function: ApplyPushback
    // purpose: Called within projectile's (prefab) onCollision method 
    //          upon activation, player's push direction is determined
    public void ApplyPushback(Vector3 direction, float force)
    {
        isBeingPushed = true;
        pushDirection = direction.normalized;

        // Calculate push velocity based on direction and force
        pushVelocity = pushDirection * force;

        // reset velocity after a while
        StartCoroutine(ResetPushback());
    }

    // function: ResetPushback
    // purpose: Gradually stops the push effect after 0.5 secs
    //          then resets the push velocity
    private IEnumerator ResetPushback()
    {
        yield return new WaitForSeconds(0.5f);
        isBeingPushed = false;
        pushVelocity = Vector3.zero;
    }
}
