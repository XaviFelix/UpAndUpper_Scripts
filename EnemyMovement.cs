/***************************************************************
*file: EnemyMovement.cs
*author: Darlyn Villanueva & Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/19/24
*
*purpose: This program controls the enemy's movement behavior. 
*         The enemy moves toward the player when within a 
*         certain range.
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;

    public float moveSpeed = 5f;  // movement speed when moving towards the player
    public float moveRange = 10f; // range where the enemy starts moving towards the player

    // function: Start
    // purpose: called before the first frame update; initializes the reference to the player's Transform
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // find the player
    }

    // function: Update
    // purpose: called once per frame; moves the enemy towards the player if within the move range
    void Update()
    {
        if(player != null)
        {
            // detect distance to player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // move only if player is within the specified range
            if(distanceToPlayer <= moveRange)
            {
                MoveTowardsPlayer();
            }
        }
    }

    // function: MoveTowardsPlayer
    // purpose: moves the enemy towards the player's position
    void MoveTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.position += new Vector3(directionToPlayer.x, 0, directionToPlayer.z) * moveSpeed * Time.deltaTime;
    }
}
