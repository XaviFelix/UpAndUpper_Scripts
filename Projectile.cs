/***************************************************************
*file: Projectile.cs
*author: Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/20/24
*
*purpose: This script handles the behavior of projectiles, 
*         including collision detection, applying a push force 
*         to the player upon contact, and destruction after impact.
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float pushForce = 10f; // amount of force added to the projectile

    // function: Start
    // purpose: called before the first frame update
    void Start()
    {

    }

    // function: Update
    // purpose: called once per frame
    void Update()
    {

    }

    // function: OnCollisionEnter
    // purpose: detects collision with the player and applies a push force in the direction of the 
    //          collision, then destoys the projectile
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // get the player controller
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                // calculate the direction from the projectile to the player
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

                // remove y direction push
                pushDirection.y = 0; 

                // calls pushback method via playercontroller reference
                playerController.ApplyPushback(pushDirection, pushForce);

                // destroy the projectile upon impact
                Destroy(gameObject);
            }
        }
    }
}
