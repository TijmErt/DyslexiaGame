using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlappyRhymesHealthManager : MonoBehaviour
{
    private int health = 3;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject gameOverScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthText.text = "Health: " + health;
    }

    [ContextMenu("Decrease Health")]
    public void DecreaseHealth()
    {
        if (health <= 0)
            return;

        health--;
        healthText.text = "Health: " + health;

        if (health <= 0)
        {
            EndGame();
        }
    }

    public void ResetHealth()
    {
        health = 3;
        healthText.text = "Health: " + health;
    }

    [ContextMenu("End Game")]
    public void EndGame()
    {
        gameOverScreen.SetActive(true);

        // Pause the game time so scene activity stops
        Time.timeScale = 0f;
    }

}
