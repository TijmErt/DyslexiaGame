using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MP_Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconImage;
    [SerializeField] private Image imageDisplay;

    private string matchKey;
    private bool isImageCard;
    
    public string hiddenCardText;
    public string cardText;

    public bool isSelected;

    public MP_CardsController cardController;
    public string MatchKey => matchKey;
    public void Setup(CardData data)
    {
        matchKey = data.matchKey;
        isImageCard = data.isImage;

        if (isImageCard)
        {
            Debug.LogWarning("Image");
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

        isSelected = true;
    }

    public void Hide()
    {
        iconImage.gameObject.SetActive(false);
        imageDisplay.gameObject.SetActive(false);

        isSelected = false;
    }
}
