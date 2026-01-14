using TMPro;
using UnityEngine;

public class DJ_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (DJ_GameManager.I == null) return;

        if (scoreText != null)
            scoreText.text = $"Score: {DJ_GameManager.I.scoreCorrect}";
    }
}
