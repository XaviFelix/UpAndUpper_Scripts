using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************
*file: BackAndForth.cs
*author: J. Torres
*class: CS 4700 – Game Development
*assignment: Final Project
*date last modified: 11/29/2024
*
*purpose: This program is added to the rods on the circular 
* platforms found in the second section of the map so that they 
*  rotate
*
****************************************************************/

public class RotationCircle : MonoBehaviour
{
   
    void Start()
    {
        
    }

   
    void Update()
    {
        transform.Rotate(100 * Time.deltaTime, 0, 0, Space.Self); //Rotates the rod on the circular platforms
    }
}
