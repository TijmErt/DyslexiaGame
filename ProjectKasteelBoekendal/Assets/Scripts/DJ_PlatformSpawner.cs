using UnityEngine;

public class DJ_PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera cam;
    public GameObject platformPrefab;
    public SO_DJ_WordData wordData;

    [Header("Spawn Settings")]
    public float rowSpacingY = 2.2f;
    public float spawnAheadY = 12f;        // how far above camera to keep spawning
    public int platformsPerRowEasy = 2;
    public int platformsPerRowHard = 3;

    [Header("X Range")]
    public float xMin = -2.5f;
    public float xMax = 2.5f;
    public float minXSeparation = 1.6f;   // keep platforms reachable and close-ish

    private float nextRowY;

    private void Start()
    {
        if (cam == null) cam = Camera.main;

        // Start spawning a bit above the player
        nextRowY = player.position.y + 1f;

        // Create a few starter rows
        for (int i = 0; i < 10; i++)
        {
            SpawnRow(nextRowY);
            nextRowY += rowSpacingY;
        }
    }

    private void Update()
    {
        HandlePlatformSpawning();
    }

    private void HandlePlatformSpawning()
    {
        if (DJ_GameManager.I != null && DJ_GameManager.I.isGameOver) return;

        float camTopY = cam.transform.position.y + spawnAheadY;
        while (nextRowY < camTopY)
        {
            SpawnRow(nextRowY);
            nextRowY += rowSpacingY;
        }
    }

    private void SpawnRow(float y)
    {
        int perRow = (DJ_GameManager.I != null && DJ_GameManager.I.scoreCorrect >= 15) ? platformsPerRowHard : platformsPerRowEasy;

        // Choose which index is correct (exactly 1 correct)
        int correctIndex = Random.Range(0, perRow);

        // Pick word entry
        int minTier = DJ_GameManager.I != null ? DJ_GameManager.I.minTier : 1;
        int maxTier = DJ_GameManager.I != null ? DJ_GameManager.I.maxTier : 1;
        var entry = wordData.GetRandomEntry(minTier, maxTier);

        // Prepare positions (simple reachability: close spacing)
        float[] xs = new float[perRow];
        xs[0] = Random.Range(xMin, xMax);

        for (int i = 1; i < perRow; i++)
        {
            float attempt = Random.Range(xMin, xMax);
            int safety = 0;
            while (Mathf.Abs(attempt - xs[i - 1]) < minXSeparation && safety < 20)
            {
                attempt = Random.Range(xMin, xMax);
                safety++;
            }
            xs[i] = attempt;
        }

        // Spawn platforms
        for (int i = 0; i < perRow; i++)
        {
            Vector3 pos = new Vector3(xs[i], y, 0f);
            GameObject obj = Instantiate(platformPrefab, pos, Quaternion.identity);

            DJ_WordPlatform wp = obj.GetComponent<DJ_WordPlatform>();

            bool isCorrect = (i == correctIndex);
            if (isCorrect)
            {
                wp.Setup(entry.correct, true);
            }
            else
            {
                // pick a wrong variant (fallback if none)
                string wrong = (entry.wrongVariants.Count > 0)
                    ? entry.wrongVariants[Random.Range(0, entry.wrongVariants.Count)]
                    : entry.correct + "?";

                wp.Setup(wrong, false);
            }
        }
    }
}
