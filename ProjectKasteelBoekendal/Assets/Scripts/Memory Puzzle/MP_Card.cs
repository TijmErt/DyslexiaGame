using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MP_Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconImage;

    public string hiddenCardText;
    public string cardText;

    public bool isSelected;

    public MP_CardsController cardController;

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
        iconImage.text = cardText;
        isSelected = true;
    }

    public void Hide()
    {
        iconImage.text = hiddenCardText;
        isSelected = false;
    }
}
