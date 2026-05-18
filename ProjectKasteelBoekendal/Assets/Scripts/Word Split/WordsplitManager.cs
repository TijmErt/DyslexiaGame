using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WordsplitManager : MonoBehaviour
{
    
    [field: SerializeField] public WordCollection WordList { get; set; }
    [field: SerializeField] public GameObject Vegetable { get; set; }
	[field: SerializeField] public GameObject HealthBar { get; set; }
    [field: SerializeField] public GameObject ScoreBar { get; set; }

    private int Score { get; set; } = 0;
    private int Health { get; set; } = 3;
    
    private List<string> CurrentWord { get; set; }
    
    /// <summary>
    /// Gets a random word from the word list
    /// </summary>
    /// <returns>The randomly selected word in a list of its syllables</returns>
    private List<string> GetRandomWord() {
        var word = this.WordList.words[Random.Range(0, this.WordList.words.Count - 1)];
        return word.syllablesParts;
    }

    void Start() {
        this.CurrentWord = this.GetRandomWord();
        this.ShowWord(string.Join("", this.CurrentWord));
    }

    public void PerformCut() {
        var vegetableManager = this.Vegetable.GetComponent<VegetableManager>();

        if (vegetableManager.CheckIfCorrect(this.CurrentWord)) {
            this.IncreaseScore();
        }
        else {
            this.DecreaseHealth();
        }
        
        this.CurrentWord = this.GetRandomWord();
        this.ShowWord(string.Join("", this.CurrentWord));
    }

    private void ShowWord(string word) {
        this.Vegetable.GetComponent<VegetableManager>().ShowWord(word);
    }

    private void DecreaseHealth() {
        var children = this.HealthBar.GetComponentsInChildren<Image>();

        if (children.Length == 1) {
            this.FinishGame();
            return;
        }
        
        children[this.Health - 2].gameObject.SetActive(false);
    }

    private void IncreaseScore() {
        this.Score++;
        this.ScoreBar.GetComponentInChildren<TextMeshProUGUI>().text = this.Score.ToString();
    }

    private void FinishGame() {
        print("Prointjjkghbaejhbkjvh");
    }
}
