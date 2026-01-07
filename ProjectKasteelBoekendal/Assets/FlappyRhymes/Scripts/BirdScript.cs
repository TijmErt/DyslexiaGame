using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputReader inputReader = default;
    [SerializeField] private GameObject wordSpawner;
    [SerializeField] private FlappyRhymesWordManager wordManager;

    private int topBoundary = 7;
    private int bottomBoundary = -5;

    void Update()
    {
        if (transform.position.y < bottomBoundary || transform.position.y > topBoundary)
        {
            rb.linearVelocity *= -1;
        }
    }

    void OnEnable()
    {
        inputReader.touchEvent += OnTouch;
        inputReader.leftMouseButtonEvent += OnMouseTap;
    }

    void OnDisable()
    {
        inputReader.touchEvent -= OnTouch;
        inputReader.leftMouseButtonEvent -= OnMouseTap;
    }
    private void OnTouch(Vector2 arg0)
    {
        Flap();
    }

    private void OnMouseTap()
    {
        Flap();
    }

    private void Flap()
    {
        rb.linearVelocity = Vector2.up * 5f;
    }

    void Start()
    {
        inputReader.EnableFlappyRhymes();

    }
}
