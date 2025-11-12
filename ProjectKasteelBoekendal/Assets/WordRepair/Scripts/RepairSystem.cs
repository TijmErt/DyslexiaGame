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

        if (correct)
        {
            switch (wordManager.currentRoundType)
            {
                case RoundType.Preparation:
                    customer.ProgressOrder();
                    resultText.text = "Well done!";
                    score += 1;
                    materials += 1;
                    break;
                case RoundType.Repair :
                    customer.ProgressOrder();
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
                    customer.ProgressOrder();
                    resultText.text = "Close!\nThe word was: " + wordManager.GetCurrentWord();
                    break;
                case RoundType.Repair:
                    customer.ProgressOrder();
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
