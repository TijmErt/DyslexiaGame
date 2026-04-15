using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MP_CardsController_Model : MonoBehaviour
{
    [SerializeField] MP_Card cardPrefab;
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private WordCollection wordCollection;
    [SerializeField] private int pairCount = 4;
    [SerializeField] private GameObject minigameEndMenu;

    private List<CardData> cardList;
    private List<string> wordPairs;

    private MP_Card firstSelected;
    private MP_Card secondSelected;


    private int matchCounts;
    private bool isChecking;
    private void Start()
    {
        PrepareCards();
        CreateCards();
    }

    private void PrepareCards()
    {
        cardList = new List<CardData>();

        List<MemoryWordData> memoryData = wordCollection.GetMemoryData(pairCount);

        foreach (var entry in memoryData)
        {
            // TEXT card
            cardList.Add(new CardData
            {
                matchKey = entry.id.ToString(),
                word = entry.word,
                isImage = false
            });

            // IMAGE card
            if (entry.image != null)
            {
                cardList.Add(new CardData
                {
                    matchKey = entry.id.ToString(),
                    image = entry.image,
                    isImage = true
                });
            }
            else
            {
                // fallback
                cardList.Add(new CardData
                {
                    matchKey = entry.id.ToString(),
                    word = entry.word,
                    isImage = false
                });
            }
        }

        Shuffle(cardList);
    }
    private void CreateCards()
    {
        List<Transform> wordSlots = new List<Transform>();
        List<Transform> imageSlots = new List<Transform>();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (i < 6) wordSlots.Add(cardSlots[i]);
            else imageSlots.Add(cardSlots[i]);
        }

        ShuffleTransforms(wordSlots);
        ShuffleTransforms(imageSlots);

        int wordIndex = 0;
        int imageIndex = 0;

        foreach (var data in cardList)
        {
            Transform parentSlot;

            if (data.isImage)
            {
                if (imageIndex >= imageSlots.Count)
                {
                    Debug.LogError("Not enough IMAGE slots for the number of image cards!");
                    continue;
                }

                parentSlot = imageSlots[imageIndex];
                imageIndex++;
            }
            else
            {
                if (wordIndex >= wordSlots.Count)
                {
                    Debug.LogError("Not enough WORD slots for the number of word cards!");
                    continue;
                }

                parentSlot = wordSlots[wordIndex];
                wordIndex++;
            }

            MP_Card card = Instantiate(cardPrefab, parentSlot);

            RectTransform rt = card.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;

            card.Setup(data);
            card.cardController = this;
        }
    }
    public void SetSelected(MP_Card card)
    {
        if (isChecking || card.isSelected) return;

        card.Show();

        if (firstSelected == null)
        {
            firstSelected = card;
            return;
        }

        secondSelected = card;
        StartCoroutine(CheckMatching(firstSelected, secondSelected));

        firstSelected = null;
        secondSelected = null;
    }
    IEnumerator CheckMatching(MP_Card a, MP_Card b)
    {
        isChecking = true;

        yield return new WaitForSeconds(0.3f);

        Debug.Log(a.MatchKey + " is " + b.MatchKey);
        if (a.MatchKey == b.MatchKey)
        {
            matchCounts++;

            if (matchCounts >= cardList.Count / 2)
            {
                minigameEndMenu.SetActive(true);
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }

        isChecking = false;
    }
    private void ShuffleTransforms(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private void Shuffle(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
