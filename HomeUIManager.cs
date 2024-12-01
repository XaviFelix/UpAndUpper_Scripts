/***************************************************************
*file: HomeUIManager.cs
*author: Joseph Setiawan
*class: CS 4700 - Game Development
*assignment: Program 4
*date last modified: 11/15/24
*
*purpose: This program handles the start button for the home menu.
*         
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class HomeUIManager : MonoBehaviour
{
    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }
}
