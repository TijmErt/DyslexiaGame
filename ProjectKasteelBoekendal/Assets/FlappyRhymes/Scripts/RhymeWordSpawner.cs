using UnityEngine;
using TMPro;

public class RhymeWordSpawner : MonoBehaviour
{

    [SerializeField] private GameObject rhymeWordPrefab;
    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private FlappyRhymesWordManager wordManager;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnRhymeWord();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRhymeWord();
            timer = 0f;
        }
    }

    private void SpawnRhymeWord()
    {
        if (rhymeWordPrefab == null || wordManager == null)
            return;

        (string, string) wordAndRhyme = wordManager.GetRandomWordAndRhyme();

        // Instantiate, then set texts on the instantiated object's children
        GameObject instance = Instantiate(rhymeWordPrefab, transform.position, Quaternion.identity);

        var texts = instance.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var t in texts)
        {
            if (t != null)
                t.text = wordAndRhyme.Item2;
        }
    }
}
