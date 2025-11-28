using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class RepairSystem : MonoBehaviour
{
    public WordManager wordManager;
    public Customer customer;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI materialsText;
    private int score = 0;
    private int materials = 5;

    public void CompleteWord()
    {
        bool correct = wordManager.CheckAnswer();

        scoreText.text = "Voltooid: " + score;

        Invoke(nameof(NextWord), 2f);
    }

    private void NextWord()
    {
        wordManager.Next();
        resultText.text = "";
    }
}
