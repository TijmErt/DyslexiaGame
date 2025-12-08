using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject popupPrefab;
    public GameObject orderPopupPrefab;
    public Transform popupParent;
    public float popupDuration = 2f;
    [SerializeField] private GameObject materialsPopup;
    [SerializeField] private GameObject feedbackPopup;

    private GameObject popupInstance;
    private GameObject currentPopupOwner;

    public void ShowFeedbackPopup(GameObject popupOwner, string message, float yOffset)
    {
        HidePopup();
        currentPopupOwner = popupOwner;
        float popupYOffset = yOffset;

        if (popupInstance == null || popupInstance.CompareTag("OrderPopup"))
        {
            // instantiate under popupParent
            popupInstance = Instantiate(popupPrefab, popupParent);
        }
        else
        {
            popupInstance.SetActive(true);
        }

        // update text inside the instantiated prefab
        var tmp = popupInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        Invoke(nameof(HidePopup), popupDuration);
    }

    public void ShowMaterialsPopup(string message)
    {
        HideFeedbackPopup();
        if (materialsPopup == null) return;
        materialsPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;
        materialsPopup.SetActive(true);
    }

    public void HideMaterialsPopup()
    {
        if (materialsPopup != null)
            materialsPopup.SetActive(false);
    }

    public void ShowFeedback(string message)
    {
        HideMaterialsPopup();
        if (feedbackPopup == null) return;
        feedbackPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;
        feedbackPopup.SetActive(true);

        Invoke(nameof(HideFeedbackPopup), popupDuration);
    }

    private void HideFeedbackPopup()
    {
        if (feedbackPopup != null)
            feedbackPopup.SetActive(false);
    }

    public void ShowOrderPopup(GameObject popupOwner, Sprite image, float xOffset, float yOffset)
    {
        HidePopup();
        currentPopupOwner = popupOwner;

        if (popupInstance == null || popupInstance.CompareTag("FeedbackPopup"))
        {
            popupInstance = Instantiate(orderPopupPrefab, popupParent);
        }
        else
        {
            popupInstance.SetActive(true);
        }

        // Find the specific Image inside the instantiated popup (child named "OrderImage")
        Image img = null;
        Transform child = popupInstance.transform.Find("Image");
        if (child != null) img = child.GetComponent<Image>();

        // fallback to first Image in children if specific child not found
        if (img == null) img = popupInstance.GetComponentInChildren<Image>();

        if (img != null)
            img.sprite = image;
        else
            Debug.LogWarning("DialogueSystem: no Image found on order popup instance to set sprite.");
    }

    private void PositionPopup(int popupYOffset = 0, int popupXOffset = 0)
    {
        if (popupInstance == null || popupParent == null) return;

        // world position a bit above the customer
        Vector3 worldPos = currentPopupOwner.transform.position + Vector3.up * popupYOffset + Vector3.right * popupXOffset;

        // get parent rect and canvas to determine the camera for conversion
        RectTransform parentRect = popupParent as RectTransform;
        Canvas parentCanvas = popupParent.GetComponentInParent<Canvas>();

        // screen point from world
        Vector3 screenPoint = Camera.main != null
            ? Camera.main.WorldToScreenPoint(worldPos)
            : new Vector3(worldPos.x, worldPos.y, 0f);

        // choose camera for ScreenPointToLocalPointInRectangle: null for Overlay, canvas.worldCamera otherwise
        Camera uiCamera = (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? parentCanvas.worldCamera
            : null;

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, uiCamera, out anchoredPos);

        var popupRect = popupInstance.GetComponent<RectTransform>();
        if (popupRect != null)
        {
            popupRect.anchoredPosition = anchoredPos;
        }
        else
        {
            popupInstance.transform.position = worldPos;
        }
    }

    private void HidePopup()
    {
        if (popupInstance != null)
        {
            popupInstance.SetActive(false);
        }
    }
}
