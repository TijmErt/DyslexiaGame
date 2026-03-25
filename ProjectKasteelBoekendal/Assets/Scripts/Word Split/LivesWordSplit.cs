using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class LivesWordSplit : MonoBehaviour
{

    public int lives = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calculates the amount of lives left and triggers end of game logic
    public void DecreaseLives()
    {
        lives--;
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            // Logic for game over
        }
    }
}
