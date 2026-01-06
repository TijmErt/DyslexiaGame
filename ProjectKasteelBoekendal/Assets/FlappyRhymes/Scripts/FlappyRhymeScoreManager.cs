using TMPro;
using UnityEngine;

public class FlappyRhymeScoreManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }

    [ContextMenu("Increase Score")]
    public void IncreaseScore()
    {
        score++;
    }
}
