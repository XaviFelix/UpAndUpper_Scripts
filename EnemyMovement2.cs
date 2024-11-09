
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy Movement: where the enemy moves around the platform- patrol movement

public class EnemyMovement2 : MonoBehaviour{


    //speed of enemy
    public float speed= 10f;

   //distance to patrol from platform
   public float distance= 3f;

   //reference to a platform
   public Transform platform;

   // keep initial position of enemy in platform
   public Vector3 initialPosition;

   void Start(){

    if(platform==null){

        platform= GameObject.FindGameObjectWithTag("Platform").transform;
    }

   // initialPosition location realtive to platform
   //inital postion is to left
   initialPosition= platform.position + new Vector3(-distance,0,0);
   transform.position= initialPosition;

   }

   void Update(){

    //new postion
    float newX= initialPosition.x + distance * Mathf.Sin(Time.time * speed);
    float newZ= initialPosition.z + distance * Mathf.Cos(Time.time * speed);

    //update position of enemy
   transform.position = new Vector3(newX, platform.position.y + platform.localScale.y / 2, newZ);


   }

}