using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class HomeUIManager : MonoBehaviour
{
    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");  // Replace "Level1" with the actual name of  game scene
    }
}