using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MP_CardsController : MonoBehaviour
{
    [SerializeField] MP_Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] private TempWordPair[] words;
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

        foreach (var pair in words)
        {
            // Text card
            cardList.Add(new CardData
            {
                matchKey = pair.word,
                word = pair.word,
                isImage = false
            });

            // Image card (if exists)
            if (pair.image != null)
            {
                cardList.Add(new CardData
                {
                    matchKey = pair.word,
                    image = pair.image,
                    isImage = true
                });
            }
            else
            {
                // fallback: duplicate word if no image
                cardList.Add(new CardData
                {
                    matchKey = pair.word,
                    word = pair.word,
                    isImage = false
                });
            }
        }

        Shuffle(cardList);
    }

    private void CreateCards()
    {
        foreach (var data in cardList)
        {
            MP_Card card = Instantiate(cardPrefab, gridTransform);
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



    private void Shuffle(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
