using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class PillarWord : MonoBehaviour
{

    private FlappyRhymeScoreManager scoreManager;
    private FlappyRhymesWordManager wordManager;

    // Public initializer to set manager references on instantiated prefabs
    public void Initialize(FlappyRhymesWordManager wm, FlappyRhymeScoreManager sm)
    {
        wordManager = wm;
        scoreManager = sm;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If this prefab was not initialized at spawn time, try to find managers in the scene
        if (wordManager == null)
            wordManager = FindFirstObjectByType<FlappyRhymesWordManager>();

        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<FlappyRhymeScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BirdScript>() != null)
        {
            if (wordManager == null || scoreManager == null)
                return;

            string word = GetComponent<TextMeshProUGUI>().text;

            Debug.Log("Collision detected with " + word);

            if (wordManager.IsRhyme(word))
                scoreManager.IncreaseScore();
        }
    }

}
