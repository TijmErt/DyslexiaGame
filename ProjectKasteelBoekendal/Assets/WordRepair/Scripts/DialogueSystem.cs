using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject popupPrefab;
    public GameObject orderPopupPrefab;
    public Transform popupParent;
    public float popupDuration = 2f;
    private float popupYOffset;
    private float popupXOffset;
    private GameObject popupInstance;
    private GameObject currentPopupOwner;

    public void ShowFeedbackPopup(GameObject popupOwner, string message, float yOffset)
    {
        HidePopup();
        currentPopupOwner = popupOwner;
        popupYOffset = yOffset;
        popupXOffset = 0f;

        if (popupInstance == null || popupInstance.CompareTag("OrderPopup"))
        {
            // instantiate under popupParent
            popupInstance = Instantiate(popupPrefab, popupParent);
        }
        else
        {
            popupInstance.SetActive(true);
        }

        PositionPopupAbove();

        // update text inside the instantiated prefab (find the TMP component)
        var tmp = popupInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        Invoke(nameof(HidePopup), popupDuration);
    }

    public void ShowOrderPopup(GameObject popupOwner, Sprite image, float xOffset, float yOffset)
    {
        HidePopup();
        currentPopupOwner = popupOwner;
        popupYOffset = yOffset;
        popupXOffset = xOffset;

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

        PositionPopupAbove();
    }

    private void PositionPopupAbove()
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
            // fallback: set world position (if popup is not a UI element)
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
