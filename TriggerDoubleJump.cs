/***************************************************************
*file: TriggerDoubleJump.cs
*author: Xavier Felix
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/29/24
*
*purpose: This program triggers upon contact with a specific platform.
*         The PlayerController's ActivateDoubleJump method is called
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoubleJump : MonoBehaviour
{
    private bool hasTriggered = false; // ensures that the conditional happens once

    // function: OnControllerColliderHit
    // purpose: detects when the model's character controller component makes contact with
    //          a platform marked as 'DoubleJumpTrigger'
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("DoubleJumpTrigger") && !hasTriggered)
        {
            hasTriggered = true;
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.ActivateDoubleJump();
        }
    }

}
