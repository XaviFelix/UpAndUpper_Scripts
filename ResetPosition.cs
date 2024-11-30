/**************************************************************************
*file: ResetPosition.cs
*author: Xavier Felix & Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/23/24
*
*purpose: To reset player's position upon contact with the ground (Terrain)
*         and adding a fade in/out effect
*         
**************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public Transform resetLocation; // Location of reset point
    public FadeEffect fadeEffect; // Reference to FadeEffect

    // function: OnControllerColliderHit
    // purpose: Upon contact with the terrain the player resets
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Terrain"))
        {
            StartCoroutine(ResetWithFade());
        }
    }

    // function: ResetWithFade
    // purpose: Resets player to starting position with a fade effect
    private IEnumerator ResetWithFade()
    {
        // Fade out upon contact terrain
        yield return StartCoroutine(fadeEffect.FadeOut());

        // Reset player back to the starting position
        transform.position = resetLocation.position;

        // Fade back in at the start position
        yield return StartCoroutine(fadeEffect.FadeIn());
    }
}
