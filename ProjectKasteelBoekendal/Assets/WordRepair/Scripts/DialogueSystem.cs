using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject feedbackPopupPrefab;
    public Transform popupParent;
    public float popupDuration = 2f;
    private float popupYOffset;
    private GameObject feedbackPopupInstance;
    private GameObject currentPopupOwner;

    public void ShowFeedbackPopup(GameObject popupOwner, string message, float yOffset = 2f)
    {
        currentPopupOwner = popupOwner;
        popupYOffset = yOffset;

        if (feedbackPopupInstance == null)
        {
            // instantiate under popupParent
            feedbackPopupInstance = Instantiate(feedbackPopupPrefab, popupParent);
        }
        else
        {
            feedbackPopupInstance.SetActive(true);
        }

        PositionPopupAbove();

        // update text inside the instantiated prefab (find the TMP component)
        var tmp = feedbackPopupInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = message;

        Invoke(nameof(HidePopup), popupDuration);
    }
    
    private void PositionPopupAbove()
    {
        if (feedbackPopupInstance == null || popupParent == null) return;

        // world position a bit above the customer
        Vector3 worldPos = currentPopupOwner.transform.position + Vector3.up * popupYOffset;

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

        var popupRect = feedbackPopupInstance.GetComponent<RectTransform>();
        if (popupRect != null)
        {
            popupRect.anchoredPosition = anchoredPos;
        }
        else
        {
            // fallback: set world position (if popup is not a UI element)
            feedbackPopupInstance.transform.position = worldPos;
        }
    }

    private void HidePopup()
    {
        if (feedbackPopupInstance != null)
        {
            feedbackPopupInstance.SetActive(false);
        }
    }
}
