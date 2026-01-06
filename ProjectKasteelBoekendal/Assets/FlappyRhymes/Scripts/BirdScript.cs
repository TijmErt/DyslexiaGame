using System;
using UnityEngine;

public class BirdScript : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private InputReader inputReader = default;

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
        rigidbody.linearVelocity = Vector2.up * 5f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputReader.EnableFlappyRhymes();
    }

    // Update is called once per frame
    void Update()
    {


    }
}
