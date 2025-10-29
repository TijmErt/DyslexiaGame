using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    public int done = 100;
    public int currentProgress;
    // public TextMeshProUGUI feedbackText;
    public GameObject feedbackPopupPrefab;
    public Transform popupParent;
    public float popupDuration = 2f;
    private GameObject feedbackPopupInstance;

    void Start()
    {
        currentProgress = 0;
    }

    public void ProgressOrder(int amount)
    {
        currentProgress += amount;

        if (feedbackPopupPrefab == null) return;

        if (feedbackPopupInstance == null)
        {
            // instantiate under popupParent if provided
            feedbackPopupInstance = Instantiate(feedbackPopupPrefab, popupParent);
        }
        else
        {
            feedbackPopupInstance.SetActive(true);
        }

        // update text inside the instantiated prefab (find the TMP component)
        var tmp = feedbackPopupInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = "Thank you!";
        
        Invoke(nameof(HidePopup), popupDuration);
    }

    private void HidePopup()
    {
        if (feedbackPopupInstance != null)
        {
            feedbackPopupInstance.SetActive(false);
        }
    }
}
