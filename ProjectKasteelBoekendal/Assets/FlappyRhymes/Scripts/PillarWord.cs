using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class PillarWord : MonoBehaviour
{

    private FlappyRhymeScoreManager scoreManager;
    private FlappyRhymesWordManager wordManager;
    private FlappyRhymesHealthManager healthManager;

    // Public initializer to set manager references on instantiated prefabs, use if more than one managers are in the scene
    public void Initialize(FlappyRhymesWordManager wm, FlappyRhymeScoreManager sm, FlappyRhymesHealthManager hm)
    {
        wordManager = wm;
        scoreManager = sm;
        healthManager = hm;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If this prefab was not initialized at spawn time, try to find managers in the scene
        if (wordManager == null)
            wordManager = FindFirstObjectByType<FlappyRhymesWordManager>();

        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<FlappyRhymeScoreManager>();

        if (healthManager == null)
            healthManager = FindFirstObjectByType<FlappyRhymesHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BirdScript>() != null)
        {
            if (wordManager == null || scoreManager == null || healthManager == null)
                return;

            string word = GetComponentInChildren<TextMeshPro>().text;

            if (wordManager.IsRhyme(word))
            {
                scoreManager.IncreaseScore();
            }
            else
            {
                healthManager.DecreaseHealth();
            }
        }
    }

}
