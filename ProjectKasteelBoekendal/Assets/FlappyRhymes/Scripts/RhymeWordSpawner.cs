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

        GameObject instance = Instantiate(rhymeWordPrefab, transform.position, Quaternion.identity);

        var texts = instance.GetComponentsInChildren<TextMeshPro>(true);
        texts[0].text = wordAndRhyme.Item1;
        texts[1].text = wordAndRhyme.Item2;
    }
}
