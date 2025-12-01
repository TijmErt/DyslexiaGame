using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [System.Serializable]
    public class WordData
    {
        public string word;
        public Sprite image;
    }

    [Header("Configuration")]
    public List<WordData> words;

    [Header("References")]

    public TileController tileController;
    public Transform letterParent;
    public GameObject letterTilePrefab;
    public GameObject letterSlotPrefab;
    public GameObject customer;

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
        List<char> scrambled = new List<char>(currentWord.ToCharArray());
        scrambled.AddRange(otherWords[0].ToUpper().ToCharArray());
        scrambled.AddRange(otherWords[1].ToUpper().ToCharArray());

        int max = GetLongestWordLength();

        if (scrambled.Count < max * 3)
        {
            // add random letters until we have enough tiles
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            System.Random rand = new System.Random();
            while (scrambled.Count < max * 3)
            {
                char randomChar = alphabet[rand.Next(alphabet.Length)];
                scrambled.Add(randomChar);
            }
        }

        scrambled.Shuffle();

        foreach (char c in scrambled)
        {
            GameObject slot = CreateSlot();
            GameObject tile = CreateTile(slot.transform);

            LetterTile lt = tile.GetComponent<LetterTile>();
            if (lt == null)
            {
                Debug.LogError($"WordManager: prefab '{letterTilePrefab.name}' does not have a LetterTile component attached.");
                continue;
            }

            lt.Setup(c, tileController);

            currentTiles.Add(lt);
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
        int wordIndex = GetRandomWordIndex(currentIndex);

        otherWords[0] = words[wordIndex].word;
        otherWords[1] = words[GetRandomWordIndex(wordIndex)].word;
    }

    private int GetRandomWordIndex(int index)
    {
        return (index + 1) % words.Count;
    }

    public (string, string, string) GetPlayerAnswer()
    {
        string result1 = "";
        string result2 = "";
        string result3 = "";

        int totalSlots = letterParent.childCount;
        int totalRows = 3;

        // Safety check to prevent division by zero
        if (totalRows == 0 || totalSlots == 0) return ("", "", "");

        int slotsPerRow = totalSlots / totalRows;

        foreach (Transform slotTransform in letterParent)
        {
            LetterTile tile = slotTransform.GetComponentInChildren<LetterTile>();

            if (tile != null)
            {
                // find where this slot is in the list
                int index = slotTransform.GetSiblingIndex();

                // find the row index (0, 1, or 2)
                int rowIndex = index / slotsPerRow;

                // assign to the correct string based on exact row index
                if (rowIndex == 0)
                {
                    result1 += tile.letterChar;
                }
                else if (rowIndex == 1)
                {
                    result2 += tile.letterChar;
                }
                else if (rowIndex == 2)
                {
                    result3 += tile.letterChar;
                }
            }
        }
        return (result1, result2, result3);
    }

    public bool CheckAnswer()
    {
        (string, string, string) playerAnswer = GetPlayerAnswer();

        Debug.Log($"Player answers: {playerAnswer.Item1}, {playerAnswer.Item2}, {playerAnswer.Item3} for word {currentWord}");

        return playerAnswer.Item1.Contains(currentWord) ||
               playerAnswer.Item2.Contains(currentWord) ||
               playerAnswer.Item3.Contains(currentWord);
    }

    public void Next()
    {
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

    private GameObject CreateTile(Transform slot)
    {
        GameObject tile = Instantiate(letterTilePrefab, slot);
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