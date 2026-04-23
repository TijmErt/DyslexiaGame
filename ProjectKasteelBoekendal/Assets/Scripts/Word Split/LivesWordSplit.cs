using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class LivesWordSplit : MonoBehaviour
{
    public WordSplitProgression wordSplitProgression;
    public int lives = 3;
    public bool isGameOver = false;

    // Calculates the amount of lives left and triggers end of game logic
    public void DecreaseLives()
    {
        if (wordSplitProgression.isModelling == true)
        {
            return; // Don't decrease lives if the player is currently modelling
        }
        lives--;
    }

    public void CheckLives()
    {
        if (lives <= 0)
        {
            isGameOver = true;
            wordSplitProgression.EndGame();
        }
    }
}
