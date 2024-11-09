using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public Animator animator;   //if enemy has animations for attacks
    public Transform player;  
    public float rangeDetection= 5f; //range which the enemy detects the "player"
    public float rangeAttack= 2f; //range which the enemy attacks the "player"
    public float cooldown= 1f; // the pause in enemy attacks
    public float timeBtwnAttacks= 0.5f; // time in between sequence of attacks

    //checks to make sure attacks are triggered or not spamming
    public float lastAttackTime=0f;
    public bool isAttacking= false;

    //check which attack sequence enemy is currently in
    public int attackIndex=0;
    public string[] attackSequence = {"Attack1","Attack2"}; //attack sequence of the enemy

    void Start(){

        animator= GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        //detects the distance between enemy and player
        if(player != null){
        float distanceToPlayer= Vector3.Distance(transform.position, player.position);

        if(distanceToPlayer <= rangeDetection){

            //if player is near the detection range, enemy will start its attack sequence/chain
            if(distanceToPlayer <= rangeAttack && Time.time >= lastAttackTime+ cooldown){

                StartCoroutine(AttackSequence());
            }else{
               // MoveToPlayer(); //if player is not in range of detection, move the enemy towards the player
            }
        }
        }
    }


/*public void MoveToPlayer(){

    Vector3 direction = (player.position - transform.position).normalized;
    transform.Translate(direction * Time.deltaTime, Space.World);
}
*/


//calls to start the enemy attack sequence
 IEnumerator AttackSequence(){

    isAttacking= true;

    //will perform the sequence of attacks to the player
    while(attackIndex < attackSequence.Length){

        string currentAttack= attackSequence[attackIndex]; 

        switch(currentAttack){

            case "Attack1":
            PerformAttack(1);
            break;

            case "Attack2":
            PerformAttack(2);
            break;

            default:
            Debug.LogWarning("attack error");
            break;
        }

        yield return new WaitForSeconds(timeBtwnAttacks);
        attackIndex++;

    }

    //reset enemy attack sequence
    attackIndex=0;
    isAttacking=false;
    lastAttackTime=Time.time;
}

 void PerformAttack(int sequenceNum){

    //performs the animation for the specific sequence number
    switch(sequenceNum){

        case 1:
        animator.SetTrigger("Attack1");
        break;

        case 2:
        animator.SetTrigger("Attack2");
        break;

        default:
        Debug.LogWarning("attack unknown");
        break;
    }
}

}
