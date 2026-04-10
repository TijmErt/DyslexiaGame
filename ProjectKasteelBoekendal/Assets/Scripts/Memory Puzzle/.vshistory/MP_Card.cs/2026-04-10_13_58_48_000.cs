using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BookTheme
{
    public Sprite closedBook;
    public Sprite openBook;
}

public class MP_Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconImage;
    [SerializeField] private Image imageDisplay;

    [SerializeField] private Image bookIcon;
    [SerializeField] private BookTheme[] bookThemes;
    [SerializeField] private Vector2 closedSize;
    [SerializeField] private Vector2 openSize;

    private string matchKey;
    private bool isImageCard;
    
    public string hiddenCardText;
    public string cardText;
    private BookTheme selectedTheme;

    public bool isSelected;

    public MP_CardsController cardController;
    public string MatchKey => matchKey;
    public void Setup(CardData data)
    {
        matchKey = data.matchKey;
        isImageCard = data.isImage;

        // picks a random book color
        selectedTheme = bookThemes[UnityEngine.Random.Range(0, bookThemes.Length)];

        if (isImageCard)
        {
            imageDisplay.sprite = data.image;
        }
        else
        {
            iconImage.text = data.word;
        }
        Hide();
    }
    public void Start()
    {
        this.transform.localScale = Vector3.one;
    }

    public void OnCardClick()
    {
        cardController.SetSelected(this.gameObject.GetComponent<MP_Card>());
    }

    public void SetCardWord(string item)
    {
        cardText = item;
    }

    public void Show()
    {
        iconImage.gameObject.SetActive(!isImageCard);
        imageDisplay.gameObject.SetActive(isImageCard);

        bookIcon.sprite = openBook;

        bookIcon.rectTransform.sizeDelta = openSize;

        isSelected = true;
    }

    public void Hide()
    {
        iconImage.gameObject.SetActive(false);
        imageDisplay.gameObject.SetActive(false);

        bookIcon.sprite = closedBook;

        bookIcon.rectTransform.sizeDelta = closedSize;

        isSelected = false;
    }
}
