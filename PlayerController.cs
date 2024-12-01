/***************************************************************
*file: PlayerController.cs
*author: Xavier Felix, Darren Banhthai, & Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/27/24
*
*purpose: This program controls the player's movement, including 
*         walking, sprinting, jumping, and backwards movement. It 
*         also handles player animations for walking, running, 
*         jumping, and waking up. The player's input is processed 
*         using WASD keys, and movement is influenced by gravity.
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // references
    private CharacterController controller;
    private Animator playerAnimator;
    public Transform camera;
    private float verticalVelocity;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;         // speed while walking
    public float turningSpeed = 8f;      // speed of turning
    public float gravity = 9.81f;        // gravity force applied to player
    public float sprintSpeed = 10f;      // speed while sprinting
    public float jumpHeight = 2f;        // height the player can jump
    public float increasedWalkSpeed = 10f; //walk speed when player has speed up enhancement
    public float increasedSprintSpeed = 30f;    //sprint speed when player has speed up enhancement

    // player's input
    private float moveInput;             // vertical movement input
    private float turnInput;             // horizontal movement input
    private bool isSprinting;            // indicates if the player is sprinting
    private bool isJumping;              // indicates if the player is jumping
    private bool isMovingBackwards;      // indicates if the player is moving backwards

    // pushback data
    private bool isBeingPushed = false;  // indicates if the player is being pushed
    private Vector3 pushDirection;       // direction of the push
    private Vector3 pushVelocity = Vector3.zero;  // velocity applied during push

    //enchancement mode booleans
    public bool canDoubleJump = false;
    public bool hasSpeedUp = false;

    //keeps track of jumps and max jumps
    private int jumpCount = 0;  // Tracks how many jumps the player has performed
    private const int maxJumps = 2;  // Maximum number of jumps 

    // function: Start
    // purpose: called before the first frame update; initializes the controller and animator references
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isBeingPushed = false;
        pushVelocity = Vector3.zero;
        playerAudio = GetComponent<PlayerSounds>();
        playerAnimator = GetComponent<Animator>();
    }

    // function: Update
    // purpose: called once per frame; gets user input and handles movement logic
    void Update()
    {
        GetUserInput();
        Movement();

        // pushback behavior
        if(isBeingPushed)
        {
            controller.Move(pushVelocity * Time.deltaTime);
        }
    }

    // function: GetUserInput
    // purpose: retrieves input from the user for movement, sprinting, and jumping
    private void GetUserInput()
    {
        moveInput = Input.GetAxis("Vertical");           // get vertical movement input
        turnInput = Input.GetAxis("Horizontal");         // get horizontal movement input
        isSprinting = Input.GetKey(KeyCode.LeftShift);   // sprint when left shift key is pressed
        isMovingBackwards = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);  // check if the player is moving backwards
        isJumping = Input.GetKeyDown(KeyCode.Space) && (controller.isGrounded || (canDoubleJump && jumpCount < 2)); // Allow jump if grounded or if double jump is active and there is room for a second jump
    }

    // function: CalcVerticalForce
    // purpose: calculates the vertical velocity for gravity and jumping
    private float CalcVerticalForce()
    {
        if(isJumping)
        {
             // Check if the player is grounded and initiate a jump or double jump
            if (controller.isGrounded)
            {
                // First jump
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                jumpCount = 1;  //record single jump
                playerAnimator.SetTrigger("IsJumping");
                playerAudio.PlayJumpSound();
            }
            else if (canDoubleJump && jumpCount < 2)  // Allow double jump if activated
            {
                // Double jump
                verticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                jumpCount = 2;  // record double jump
                playerAnimator.SetTrigger("IsJumping");
                playerAudio.PlayJumpSound();
            }
        }
        else 
        {
            // apply gravity if not jumping and grounded
            if(!controller.isGrounded)
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
            else
            {
                verticalVelocity = -1f;
                jumpCount = 0;  // Reset jump count when grounded

            }
        }
        return verticalVelocity;
    }

    // function: PlayerMovementAnimation
    // purpose: handles player movement animations based on the movement state (walking, running, jumping, etc.)
    private void PlayerMovementAnimation()
    {
        if (playerAudio == null || controller == null || playerAnimator == null)
        {
            Debug.LogError("Missing components! Ensure playerAudio, controller, and playerAnimator are assigned.");
            return; // Exit early if any component is null
        }

        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = transform.TransformDirection(move);

        //get current walking/sprinting speed based on whether player has enhancement
        float currentSprintingSpeed = hasSpeedUp ? increasedSprintSpeed : sprintSpeed;
        float currentWalkingSpeed = hasSpeedUp ? increasedWalkSpeed : walkSpeed;

        //get current speed on whether player is sprinting or not
        float currentSpeed = isSprinting ? currentSprintingSpeed : currentWalkingSpeed;

        move *= currentSpeed; // Scale X and Z components by the speed
        move.y = CalcVerticalForce();                   // apply vertical movement (gravity or jump)
        controller.Move(move * Time.deltaTime);

        // check if the player is moving and update the Animator
        bool isMoving = moveInput != 0 || turnInput != 0;
        playerAnimator.SetBool("IsWalking", isMoving);

        // play running animation only when sprinting and moving
        playerAnimator.SetBool("IsRunning", isSprinting && isMoving);

        // walking and running backwards animations
        if(isMovingBackwards)
        {
            if(isSprinting)
            {
                playerAnimator.SetBool("IsRunningBackwards", true);   // play running backwards animation
                playerAnimator.SetBool("IsWalkingBackwards", false);  // stop walking backwards animation
            }
            else
            {
                playerAnimator.SetBool("IsWalkingBackwards", true);   // play walking backwards animation
                playerAnimator.SetBool("IsRunningBackwards", false);  // stop running backwards animation
            }
        }
        else
        {
            playerAnimator.SetBool("IsWalkingBackwards", false);  // stop walking backwards animation
            playerAnimator.SetBool("IsRunningBackwards", false);  // stop running backwards animation
        }

        // stop jump animation when grounded
        if(controller.isGrounded && !isJumping)
        {
            playerAnimator.ResetTrigger("IsJumping");  // reset jump animation when grounded
        }
    }

    // function: PlayerLookDirection
    // purpose: adjusts the player's rotation to face the direction of movement
    private void PlayerLookDirection()
    {
        if(Mathf.Abs(turnInput) > 0 || Mathf.Abs(moveInput) > 0)
        {
            Vector3 currentLookDirection = camera.forward; // get the camera's forward direction
            currentLookDirection.y = 0;                    // keep the rotation on the horizontal plane
            Quaternion playerRotation = Quaternion.LookRotation(currentLookDirection);  // calculate rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Time.deltaTime * turningSpeed);  // apply rotation smoothly
        }
    }

    // function: Movement
    // purpose: handles all movement-related logic, including input, animation, and character rotation
    private void Movement()
    {
        PlayerMovementAnimation();  // player animation based on user input
        PlayerLookDirection();      // adjusts player rotation to move in the correct direction
    }

    // function: ApplyPushback
    // purpose: called to apply a pushback force to the player
    public void ApplyPushback(Vector3 direction, float force)
    {
        isBeingPushed = true;
        pushDirection = direction.normalized;

        // calculate push velocity based on direction and force
        pushVelocity = pushDirection * force;

        // reset velocity after a while
        StartCoroutine(ResetPushback());
    }

    // function: ResetPushback
    // purpose: gradually stops the push effect after 0.5 seconds and resets the push velocity
    private IEnumerator ResetPushback()
    {
        yield return new WaitForSeconds(0.5f);
        isBeingPushed = false;
        pushVelocity = Vector3.zero;
    }

     //function: ActivateDoubleJump
    //purpose: enable double jump for player
    public void ActivateDoubleJump()
    {
        canDoubleJump = true;
        Debug.Log("Double jump active");
    }

    //function: ActivateSpeedUp
    //purpose: enable speed increase for player
     public void ActivateSpeedUp()
    {
        hasSpeedUp = true;
        Debug.Log("Speed active");
    }

    //function: ActivateJumpHeightIncrease
    //purpose: increase jump height for player
    public void ActivateJumpHeightIncrease(){
        jumpHeight = 6.5f;
        Debug.Log("Jump Height Increase active");
    }
}
