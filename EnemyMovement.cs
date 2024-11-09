
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyMovement: where the enemy is moving vertically

public class EnemyMovement : MonoBehaviour{

    //range of the movement of enemy
    public float rangeX= 5f;

    //speed of enemy
    public float speed= 10f;

    //initial direction
    public float direction= 1f;

   // keep initial position of enemy
   public Vector3 initialPosition;

   void Start(){

   // initialPosition location in Y
   initialPosition= transform.position;

   }

   void Update(){

    float movementX= direction * speed* Time.deltaTime;

    //new postion
    float newX= transform.position.x + movementX;

    //check if limit of area is passed or not
    if(Mathf.Abs(newX - initialPosition.x) > rangeX){

        //move in other direction
        direction *= -1;

    }else{
        
        //move the enemy forward
        transform.position= new Vector3(newX, transform.position.y, transform.position.z);

   }

}
}