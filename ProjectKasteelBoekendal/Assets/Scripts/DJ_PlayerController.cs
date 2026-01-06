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
        inputReader.mousePosEvent += OnMouseMove;
        inputReader.leftMouseButtonEvent += OnMouseTap;
    }
    private void OnDisable()
    {
        inputReader.touchEvent -= OnTouch;
        inputReader.mousePosEvent -= OnMouseMove;
        inputReader.leftMouseButtonEvent -= OnMouseTap;
    }

    private void Update()
    {
        moveDir = 0;
    }

    private void FixedUpdate()
    {
        if (DJ_GameManager.I != null && DJ_GameManager.I.isGameOver) return;

        rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocity.y);
    }

    private void ProcessTap(Vector2 pressPos)
    {
        if (DJ_GameManager.I != null && DJ_GameManager.I.isGameOver) return;

        moveDir = (pressPos.x < Screen.width * 0.5f) ? -1 : 1;

        // Game over check (falling too much)
        if (cam != null)
        {
            float camY = cam.transform.position.y;
            if (transform.position.y < camY - gameOverBelowCamera)
            {
                DJ_GameManager.I?.GameOver();
            }
        }
    }

    // Called by platforms when a correct landing happens
    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    // --- Touch Input ---
    private void OnTouch(Vector2 screenPosition)
    {
        ProcessTap(screenPosition);
    }

    // --- Mouse Input ---
    private void OnMouseMove(Vector2 position)
    {
        currentMousePos = position;
    }

    private void OnMouseTap()
    {
        ProcessTap(currentMousePos);
    }
}
