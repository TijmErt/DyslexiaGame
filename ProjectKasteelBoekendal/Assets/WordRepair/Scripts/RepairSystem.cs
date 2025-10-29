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

    public int correct = 30;
    public int wrong = 10;
    private int score = 0;

    private int materials = 5;

    public void CompleteWord()
    {
        bool correct = wordManager.CheckAnswer();

        if (correct)
        {
            switch (wordManager.currentRoundType)
            {
                case RoundType.Preparation:
                    customer.ProgressOrder(this.correct);
                    resultText.text = "Well done!";
                    materials += 1;
                    break;
                case RoundType.Repair :
                    customer.ProgressOrder(this.correct);
                    resultText.text = "Perfect!";
                    score += 1;
                    materials -= 1;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (wordManager.currentRoundType)
            {
                case RoundType.Preparation:
                    customer.ProgressOrder(wrong);
                    resultText.text = "Close!\nThe word was: " + wordManager.GetCurrentWord();
                    break;
                case RoundType.Repair:
                    customer.ProgressOrder(wrong);
                    materials -= 1;
                    resultText.text = "Close!\nThe word was: " + wordManager.GetCurrentWord();
                    break;
                default:
                    break;
            }
            
        }

        scoreText.text = "Completed: " + score;

        materialsText.text = "Materials: " + materials;

        Invoke(nameof(NextWord), 2f);
    }

    private void NextWord()
    {
        wordManager.Next();
        resultText.text = "";
    }
}
