using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_CardsController : MonoBehaviour
{
    [SerializeField] MP_Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] private string[] words;
    [SerializeField] private GameObject minigameEndMenu;

    private List<string> wordPairs;

    private MP_Card firstSelected;
    private MP_Card secondSelected;

    private int matchCounts;

    private void Start()
    {
        PrepareCards();
        CreateCards();
    }

    private void PrepareCards()
    {
        wordPairs = new List<string>();
        for (int i = 0; i < words.Length; i++)
        {
            // Added 2 times to make pairs
            wordPairs.Add(words[i]);
            wordPairs.Add(words[i]);
        }

        ShuffleWords(wordPairs);
    }

    private void CreateCards()
    {
        for (int i = 0; i < wordPairs.Count; i++)
        {
            MP_Card card = Instantiate(cardPrefab, gridTransform);
            card.SetCardWord(wordPairs[i]);
            card.cardController = this;
            card.transform.localScale = Vector3.one;
        }
    }

    public void SetSelected(MP_Card card)
    {
        if (card.isSelected == false)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }
            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatching(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }
    }

    IEnumerator CheckMatching(MP_Card a, MP_Card b)
    {
        yield return new WaitForSeconds(0.3f);
        if (a.cardText == b.cardText)
        {
            // Matched
            matchCounts++;
            if (matchCounts >= wordPairs.Count / 2)
            {
                // Level complete
                minigameEndMenu.SetActive(true);
            }
        }
        else
        {
            // Not matched
            a.Hide();
            b.Hide();
        }
    }

    private void ShuffleWords(List<string> wordList)
    {
        for (int i = wordList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            string temp = wordList[i];
            wordList[i] = wordList[randomIndex];
            wordList[randomIndex] = temp;
        }
    }
}
