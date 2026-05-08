using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordsplitManager : MonoBehaviour
{
    
    [field: SerializeField] public WordCollection WordList { get; set; }
    [field: SerializeField] public GameObject Vegetable { get; set; }
    
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
        this.Vegetable.GetComponent<VegetableManager>().ShowWord(string.Join("", this.CurrentWord));
    }

    public void PerformCut() {
        var vegetableManager = this.Vegetable.GetComponent<VegetableManager>();
        
        print(vegetableManager.CheckIfCorrect(this.CurrentWord) ? "CORRECT" : "WRONGNGGGN");
    }
}
