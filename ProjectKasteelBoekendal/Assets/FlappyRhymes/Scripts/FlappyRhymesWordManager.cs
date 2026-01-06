using TMPro;
using UnityEngine;

public class FlappyRhymesWordManager : MonoBehaviour
{

    private WordRhymes currentWordRhymes;
    [SerializeField] private TextMeshProUGUI wordText;

    private string[] words = { "muis", "huis", "keus", "leus", "pluis" };

    void Awake()
    {
        currentWordRhymes = new WordRhymes();
        wordText.text = currentWordRhymes.word;
    }

    public (string, string) GetRandomWordAndRhyme()
    {
        string randomRhyme = currentWordRhymes.rhymes[Random.Range(0, currentWordRhymes.rhymes.Length)];
        string randomWord = words[Random.Range(0, words.Length)];
        return (randomWord, randomRhyme);
    }

    public bool IsRhyme(string word)
    {
        if (string.IsNullOrWhiteSpace(word) || currentWordRhymes?.rhymes == null)
            return false;

        return System.Array.Exists(currentWordRhymes.rhymes,
            rhyme => string.Equals(rhyme, word, System.StringComparison.OrdinalIgnoreCase));
    }
}
