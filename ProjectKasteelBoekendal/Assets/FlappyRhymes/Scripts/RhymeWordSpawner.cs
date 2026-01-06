using UnityEngine;
using TMPro;

public class RhymeWordSpawner : MonoBehaviour
{

    [SerializeField] private GameObject rhymeWordPrefab;
    [SerializeField] private float spawnInterval = 3f;

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

        if (texts != null && texts.Length >= 2)
        {
            if (Random.value < 0.5f)
            {
                texts[0].text = wordAndRhyme.Item1;
                texts[1].text = wordAndRhyme.Item2;
            }
            else
            {
                texts[0].text = wordAndRhyme.Item2;
                texts[1].text = wordAndRhyme.Item1;
            }
        }
        else if (texts != null)
        {
            foreach (var t in texts)
                if (t != null)
                    t.text = wordAndRhyme.Item2;
        }
    }
}
