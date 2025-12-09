using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [System.Serializable]
    private class WordData
    {
        public string word;
        public Sprite image;
    }

    [SerializeField] private List<WordData> words;

    [SerializeField] private TileController tileController;
    [SerializeField] private Transform answerParent;
    [SerializeField] private Transform letterParent;
    [SerializeField] private GameObject letterTilePrefab;
    [SerializeField] private GameObject emptyTilePrefab;
    [SerializeField] private GameObject letterSlotPrefab;
    [SerializeField] private GameObject customer;

    private string currentWord;
    private string[] otherWords = new string[2];
    private List<LetterTile> currentTiles = new List<LetterTile>();
    private int currentIndex = 0;

    void Start()
    {
        LoadNextWord();
    }

    public void LoadNextWord()
    {
        if (currentIndex >= words.Count) currentIndex = 0;

        // remove old slots/tiles
        foreach (Transform child in letterParent)
            Destroy(child.gameObject);
        foreach (Transform child in answerParent)
            Destroy(child.gameObject);
        currentTiles.Clear();

        // get current word data
        WordData currentWordData = words[currentIndex];
        currentWord = currentWordData.word.ToUpper();

        GetOtherWords();

        SetCustomerRequestImage(currentWordData);

        MixUpTiles();
    }

    private void SetCustomerRequestImage(WordData currentWordData)
    {
        if (customer != null)
        {
            Customer custScript = customer.GetComponent<Customer>();
            if (custScript != null)
            {
                custScript.orderImage = currentWordData.image;
                custScript.NewOrder();
            }
        }
    }

    private void MixUpTiles()
    {
        int otherWordCount = 2;

        List<char> scrambled = new List<char>(currentWord.ToCharArray());
        scrambled.AddRange(otherWords[0].ToUpper().ToCharArray());

        int max = GetLongestWordLength();

        if (scrambled.Count < max * otherWordCount)
        {
            // add random letters until we have enough tiles
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            System.Random rand = new System.Random();
            while (scrambled.Count < max * otherWordCount)
            {
                char randomChar = alphabet[rand.Next(alphabet.Length)];
                scrambled.Add(randomChar);
            }
        }

        scrambled.Shuffle();

        CreateEmptyTile();
        CreateLetterTile(scrambled);

    }

    private void CreateLetterTile(List<char> charList)
    {
        foreach (char c in charList)
        {
            GameObject slot = CreateSlot();
            GameObject tile = CreateTile(slot.transform);

            LetterTile letterTile = tile.GetComponent<LetterTile>();
            if (letterTile == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            letterTile.Setup(c, tileController);

            currentTiles.Add(letterTile);
        }
    }

    private void CreateEmptyTile()
    {
        foreach (char c in currentWord)
        {
            GameObject slot = CreateAnswerSlot();
            GameObject tile = CreateEmptyTile(slot.transform);

            LetterTile letterTile = tile.GetComponent<LetterTile>();
            if (letterTile == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            letterTile.SetUpEmpty(tileController);

            currentTiles.Add(letterTile);
        }
    }

    private int GetLongestWordLength()
    {
        int maxLength = currentWord.Length;
        for (int i = 0; i < otherWords.Length; i++)
        {
            if (otherWords[i].Length > maxLength)
                maxLength = otherWords[i].Length;
        }

        return maxLength;
    }

    private void GetOtherWords()
    {
        int wordIndex = GetRandomIndex(currentIndex);

        otherWords[0] = words[wordIndex].word;
        otherWords[1] = words[GetRandomIndex(wordIndex)].word;
    }

    private int GetRandomIndex(int index)
    {
        return (index + 1) % words.Count;
    }

    public string GetPlayerAnswer()
    {
        string result = "";

        foreach (Transform slotTransform in answerParent)
        {
            LetterTile tile = slotTransform.GetComponentInChildren<LetterTile>();

            if (tile != null)
            {
                result += tile.letterChar;
            }
        }
        return result;
    }

    public bool CheckAnswer()
    {
        string playerAnswer = GetPlayerAnswer();

        return playerAnswer.Contains(currentWord);
    }

    public void Next()
    {
        string playerAnswer = GetPlayerAnswer();

        Debug.Log($"Player answers: {playerAnswer} for word {currentWord}");

        currentIndex++;

        LoadNextWord();
    }

    private GameObject CreateSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, letterParent);
        RectTransform slotRt = slot.GetComponent<RectTransform>();
        if (slotRt != null) slotRt.anchoredPosition = Vector2.zero;
        return slot;
    }

    private GameObject CreateAnswerSlot()
    {
        GameObject slot = Instantiate(letterSlotPrefab, answerParent);
        RectTransform slotRt = slot.GetComponent<RectTransform>();
        if (slotRt != null) slotRt.anchoredPosition = Vector2.zero;
        return slot;
    }

    private GameObject CreateTile(Transform slot)
    {
        GameObject tile = Instantiate(letterTilePrefab, slot);
        RectTransform tileRt = tile.GetComponent<RectTransform>();
        if (tileRt != null) tileRt.anchoredPosition = Vector2.zero;
        return tile;
    }

    private GameObject CreateEmptyTile(Transform slot)
    {
        GameObject tile = Instantiate(emptyTilePrefab, slot);
        RectTransform tileRt = tile.GetComponent<RectTransform>();
        if (tileRt != null) tileRt.anchoredPosition = Vector2.zero;
        return tile;
    }
}

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}