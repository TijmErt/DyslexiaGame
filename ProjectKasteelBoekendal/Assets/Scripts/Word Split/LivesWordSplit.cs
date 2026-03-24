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

    public void DecreaseLives()
    {
        lives--;
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            // You can add additional game over logic here, such as restarting the game or showing a game over screen.
        }
    }
}
