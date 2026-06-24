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
    [field: SerializeField] public GameObject CutButton { get; set; }

    private int Score { get; set; } = 0;
    private int Health { get; set; } = 3;
    
    private AudioSource AudioSource { get; set; }
    private SceneMediator SceneMediator { get; set; }
    
    private List<string> CurrentWord { get; set; }
    private bool FeedbackActive { get; set; } = false;
    
    /// <summary>
    /// Gets a random word from the word list
    /// </summary>
    /// <returns>The randomly selected word in a list of its syllables, cannot return the same word twice in a row</returns>
    private List<string> GetRandomWord() {
        var word = this.WordList.words[Random.Range(0, this.WordList.words.Count - 1)];

        if (this.CurrentWord == null || this.CurrentWord.Count == 0) return word.syllablesParts;
        
        return string.Join("", this.CurrentWord) == string.Join("", word.syllablesParts) ? this.GetRandomWord() : word.syllablesParts;

    }

    void Start() {
        this.AudioSource = this.GetComponent<AudioSource>();
        this.SceneMediator = FindFirstObjectByType<SceneMediator>();
        
        this.CurrentWord = this.GetRandomWord();
        this.ShowWord(string.Join("", this.CurrentWord));
    }

    public void PerformCut() {
        var vegetableManager = this.Vegetable.GetComponent<VegetableManager>();

        if (this.FeedbackActive) {
            this.HideFeedback();
            
            this.CurrentWord = this.GetRandomWord();
            this.ShowWord(string.Join("", this.CurrentWord));
            return;
        }
        
        var correctCheck = vegetableManager.CheckIfCorrect(this.CurrentWord);
        if (correctCheck.check) {
            this.AudioSource.clip = Resources.Load<AudioClip>("Audio/SoundEffects/CorrectAnswer");
            this.AudioSource.Play();
            
            this.IncreaseScore();
        }
        else {
            this.AudioSource.clip = Resources.Load<AudioClip>("Audio/SoundEffects/WrongAnswer");
            this.AudioSource.Play();
            
            this.DecreaseHealth();
        }

        this.ShowFeedback(correctCheck);
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
        this.SceneMediator.LoadPreviousScene();
    }

    private void ShowFeedback((bool check, string answer, string correct) correctCheck) {
        this.FeedbackActive = true;
        
        this.CutButton.GetComponentInChildren<TextMeshProUGUI>().text = "Verder";
        this.Vegetable.GetComponent<VegetableManager>().ShowFeedback(correctCheck);
    }
    
    private void HideFeedback() {
        this.FeedbackActive = false;
        
        this.CutButton.GetComponentInChildren<TextMeshProUGUI>().text = "Snij";
        this.Vegetable.GetComponent<VegetableManager>().HideFeedback();
    }
}
