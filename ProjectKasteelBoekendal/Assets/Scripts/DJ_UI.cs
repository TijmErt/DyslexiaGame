using TMPro;
using UnityEngine;

public class DJ_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (DJ_GameManager.I == null) return;

        if (scoreText != null)
            scoreText.text = $"Score: {DJ_GameManager.I.scoreCorrect}";

        if (resultText == null) return;

        if (DJ_GameManager.I.isWin)
            resultText.text = "You Win!";
        else if (DJ_GameManager.I.isGameOver)
            resultText.text = "Game Over";
        else
            resultText.text = "";
    }
}
