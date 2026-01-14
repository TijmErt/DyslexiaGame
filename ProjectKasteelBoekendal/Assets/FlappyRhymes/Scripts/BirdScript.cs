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
