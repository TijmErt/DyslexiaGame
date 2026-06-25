using UnityEngine;

public class RhymeWordScript : MonoBehaviour
{

    [SerializeField] private int movementSpeed = 3;

    private int screenLeftBound = -20;

    void Update()
    {
        transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);

        if (transform.position.x <= screenLeftBound)
        {
            Destroy(gameObject);
        }
    }
}
