using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BookThemeModel
{
    public Sprite closedBook;
    public Sprite openBook;
}

public class MP_Card_Model : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconImage;
    [SerializeField] private Image imageDisplay;

    [SerializeField] private Image bookIcon;
    [SerializeField] private BookThemeModel[] bookThemes;
    [SerializeField] private Vector2 closedSize;
    [SerializeField] private Vector2 openSize;

    private string matchKey;
    private bool isImageCard;

    public string hiddenCardText;
    public string cardText;
    private BookThemeModel selectedTheme;

    public bool isSelected;

    public MP_CardsController_Model cardController;
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

        // Start OPEN but NOT selected
        Show();
        isSelected = false;
    }

    public void Start()
    {
        this.transform.localScale = Vector3.one;
    }

    public void OnCardClick()
    {
        if (cardController == null)
            return;

        cardController.SetSelected(this);
    }

    public void SetCardWord(string item)
    {
        cardText = item;
    }

    public void Show()
    {
        iconImage.gameObject.SetActive(!isImageCard);
        imageDisplay.gameObject.SetActive(isImageCard);

        bookIcon.sprite = selectedTheme.openBook;
        bookIcon.rectTransform.sizeDelta = openSize;
    }

    public void Hide()
    {
        iconImage.gameObject.SetActive(false);
        imageDisplay.gameObject.SetActive(false);

        bookIcon.sprite = selectedTheme.closedBook;
        bookIcon.rectTransform.sizeDelta = closedSize;

        isSelected = false;
        GetComponent<Button>().interactable = false;
    }
}