/***************************************************************
*file: EnemyAttack.cs
*author: Darlyn Villanueva & Marie Philavong
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/19/24
*
*purpose: This program controls the enemy's ability to detect 
*         the player, shoot projectiles when the player is within 
*         range, and push the player back when nearby.
*         
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float lastAttackTime;           // tracks time of last projectile shot

    public Transform player;            
    public GameObject projectilePrefab; 
    public Transform shootPoint;        

    public float pushRange = 10f;           // range where enemy moves and pushes the player
    public float shootRange = 20f;          // range where enemy shoots projectiles at the player
    public float pushForce = 5f;            // force applied to push the player
    public float projectileSpeed = 10f;     // speed of the projectile
    public float cooldown = 1f;             // cooldown time between projectile shots

    public Transform playerChest;           // reference to player's chest

    // function: Start
    // purpose: called before the first frame update
    void Start()
    {

    }

    // function: Update
    // purpose: called once per frame; determines if the player is within push or shooting range
    //          and executes the corresponding behavior
    void Update()
    {
        if(player != null)
        {
            // detect distance to player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if(distanceToPlayer <= pushRange)
            {
                PushPlayer(); // pushes the player if within push range
            }
            else if(distanceToPlayer <= shootRange && Time.time >= lastAttackTime + cooldown)
            {
                ShootProjectile(); // shoots a projectile at the player if within shooting range
                lastAttackTime = Time.time;
            }
        }
    }

    // function: ShootProjectile
    // purpose: instantiates and launches a projectile towards the player
    private void ShootProjectile()
    {
        // create the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // target the player's chest 
        Vector3 target = playerChest != null ? playerChest.position : player.position;

        // calculate the direction to the target point
        Vector3 direction = (target - shootPoint.position).normalized;

        // apply velocity to the projectile
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        Debug.Log("Enemy is shooting projectile at player's chest.");
    }

    // TODO: Player does not use a rigidbody, it uses a character controller component. Finish later
    // function: PushPlayer
    // purpose: applies a force to push the player off the edge when within push range
    private void PushPlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if(playerRb != null)
        {
            // calculate the push direction
            Vector3 pushDirection = (player.position - transform.position).normalized;

            // apply force to push the player
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Force);

            Debug.Log("Enemy is pushing the player.");
        }
    }
}
