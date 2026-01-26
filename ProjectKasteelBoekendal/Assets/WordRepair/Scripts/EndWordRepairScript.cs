using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndWordRepairScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }
}


