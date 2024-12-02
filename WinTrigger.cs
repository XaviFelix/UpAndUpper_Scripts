/***************************************************************
*file: WinTrigger.cs
*author: Joseph Setiawan
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 12/1/24
*
*purpose: This program displays the win message and confetti effect
*          when the player reaches the final platform.
*         
****************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public Text winMessage;         // Reference to the "You Win" message
    public GameObject confetti;     // Reference to the confetti effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure player tag is "Player"
        {
            // Show the "You Win" message
            winMessage.gameObject.SetActive(true);
            
            // Play the confetti effect
            confetti.SetActive(true);    // Make confetti visible
            var particleSystem = confetti.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();  // Play the confetti particle system
            }
        }
    }
}
