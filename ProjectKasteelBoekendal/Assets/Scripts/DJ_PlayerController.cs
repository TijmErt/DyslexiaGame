using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Rigidbody))]
public class DJ_PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader = default;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;          // horizontal speed
    [SerializeField] private float jumpVelocity = 12f;      // vertical bounce strength

    [Header("Fall Game Over")]
    [SerializeField] private float gameOverBelowCamera = 7f; // if player is this far below camera -> game over

    private Rigidbody rb;

    private bool isPressed = false;
    private int moveDir = 0;

    private Vector2 currentMousePos;

    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.touchEvent += OnTouch;
        inputReader.touchPressEvent += OnTouchPress;

        inputReader.mousePosEvent += OnMouseMove;
        inputReader.mousePressEvent += OnMouseTap;
    }
    private void OnDisable()
    {
        inputReader.touchEvent -= OnTouch;
        inputReader.touchPressEvent -= OnTouchPress;

        inputReader.mousePosEvent -= OnMouseMove;
        inputReader.mousePressEvent -= OnMouseTap;
    }

    private void FixedUpdate()
    {
        float vx = isPressed ? moveDir * moveSpeed : 0f;
        rb.linearVelocity = new Vector3(vx, rb.linearVelocity.y, 0f);
    }

    private void UpdateMoveDirFromPos(Vector2 pos)
    {
        moveDir = (pos.x < Screen.width * 0.5f) ? -1 : 1;
    }

    // Called by platforms when a correct landing happens
    public void Bounce()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, 0f);
    }

    // --- Touch Input ---
    private void OnTouch(Vector2 pos)
    {
        currentMousePos = pos;
        if (isPressed) UpdateMoveDirFromPos(pos);
    }

    private void OnTouchPress(bool pressed)
    {
        Debug.LogWarning("THIS IS WHAT THE ONTOUCHPRESS EVENT IS:" + pressed);
        isPressed = pressed;
        if (!pressed) moveDir = 0;
        else UpdateMoveDirFromPos(currentMousePos); // optional
    }

    // --- Mouse Input ---
    private void OnMouseMove(Vector2 pos)
    {
        currentMousePos = pos;
        if (isPressed) UpdateMoveDirFromPos(pos);
    }

    private void OnMouseTap(bool pressed)
    {
        isPressed = pressed;
        if (!pressed) moveDir = 0;
        else UpdateMoveDirFromPos(currentMousePos);
    }
}
