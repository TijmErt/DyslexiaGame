using System;
using NUnit.Framework;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Currency;
using Managers.Quest;
using UnityEngine;
using Random = UnityEngine.Random;

public class MP_CardsController_Model : MonoBehaviour
{
    [SerializeField] MP_Card_Model cardPrefab;
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private WordCollection wordCollection;
    [SerializeField] private int pairCount = 4;
    [SerializeField] private GameObject minigameEndMenu;
    [SerializeField] private int roundsToPlay = 3;
    
    [SerializeField] private QuestTarget _QuestTarget;
    [SerializeField] private QuestMediator _QuestMediator;
    [SerializeField] private CurrencyMediator _CurrencyMediator;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip correctSound;
    
    private int roundsPlayed = 0;

    private List<CardData> cardList;

    private MP_Card_Model firstSelected;
    private MP_Card_Model secondSelected;

    private int matchCounts;
    private bool isChecking;
    private void Start()
    {
        if(_QuestTarget == null) _QuestTarget = GetComponent<QuestTarget>();
        if(_QuestMediator == null) _QuestMediator = FindFirstObjectByType<QuestMediator>();
        if(_CurrencyMediator == null) _CurrencyMediator = FindFirstObjectByType<CurrencyMediator>();
        PrepareCards();
        CreateCards();
    }
    private void PrepareCards()
    {
        cardList = new List<CardData>();

        List<MemoryWordData> memoryData = wordCollection.GetMemoryData(pairCount);

        foreach (var entry in memoryData)
        {
            cardList.Add(new CardData
            {
                matchKey = entry.id.ToString(),
                word = entry.word,
                isImage = false
            });

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
                    continue;

                parentSlot = imageSlots[imageIndex];
                imageIndex++;
            }
            else
            {
                if (wordIndex >= wordSlots.Count)
                    continue;

                parentSlot = wordSlots[wordIndex];
                wordIndex++;
            }
            MP_Card_Model card = Instantiate(cardPrefab, parentSlot);

            RectTransform rt = card.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;

            card.Setup(data);
            card.cardController = this;
        }
    }
    public void SetSelected(MP_Card_Model card)
    {
        if (isChecking || card.isSelected)
            return;

        card.Hide();
        card.isSelected = true;
        UIAudio.Play(closeSound);
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
    IEnumerator CheckMatching(MP_Card_Model a, MP_Card_Model b)
    {
        isChecking = true;

        yield return new WaitForSeconds(0.3f);

        if (a.MatchKey == b.MatchKey)
        {
            matchCounts++;
            NotifyQuest(QuestEnums.ObjectiveType.CompleteAmount, 1);
            UIAudio.Play(correctSound);
            
            if (matchCounts >= cardList.Count / 2)
            {
                roundsPlayed++;

                if (roundsPlayed < roundsToPlay)
                {
                    ResetBoard();
                }
                else
                {
                    NotifyQuest(QuestEnums.ObjectiveType.Interact, 1);
                    _CurrencyMediator.AddCurrency("KitchCoin",10);
                    minigameEndMenu.SetActive(true);
                }
            }
        }
        else
        {
            UIAudio.Play(wrongSound);
            
            yield return new WaitForSeconds(1.25f);
            a.Show();
            b.Show();

            a.isSelected = false;
            b.isSelected = false;
            UIAudio.Play(openSound);
            a.GetComponent<Button>().interactable = true;
            b.GetComponent<Button>().interactable = true;
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
    private void ResetBoard()
    {
        foreach (Transform slot in cardSlots)
        {
            if (slot.childCount > 0)
                Destroy(slot.GetChild(0).gameObject);
        }

        firstSelected = null;
        secondSelected = null;
        matchCounts = 0;
        isChecking = false;

        PrepareCards();
        CreateCards();
    }

    private void NotifyQuest(QuestEnums.ObjectiveType type,int amount)
    {
        _QuestMediator.NotifyQuest(type, _QuestTarget.targetID,amount);
    }
}