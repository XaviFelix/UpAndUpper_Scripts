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
*purpose: This program is added to the orbs attatched to rods and
* flat panels in found in the second section of the map, in order
* to give them back and forth movemnet.
*
****************************************************************/

public class BackAndForth : MonoBehaviour
{
    [SerializeField] float dist = 25;  //Movement distance
    [SerializeField] float speed = 2;  //Speed of object
    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;   //Starting position of object
    }

    void Update()
    {
        Vector3 v = startingPos;            //set starting position
        v.x += dist * Mathf.Sin(Time.time * speed); //movement along x axis
        transform.position = v;
    }
}
