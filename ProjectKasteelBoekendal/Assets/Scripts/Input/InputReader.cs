using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject,
    InputActions.IPlayerActions,
    InputActions.IUIActions,
    InputActions.ICookingWithWordsActions,
    InputActions.IFlappyRhymesActions
{
    public InputActions inputActions { get; private set; }

    public event UnityAction<Vector2> touchEvent;
    public event UnityAction<bool> touchPressEvent;

    public event UnityAction leftMouseButtonEvent;
    public event UnityAction<Vector2> mousePosEvent;
    public event UnityAction<bool> mousePressEvent;
    public event UnityAction<Vector2> moveEvent;

    private void OnEnable()
    {
        if (inputActions == null)
            inputActions = new InputActions();

        EnableGameplay();
    }

    private void OnDisable()
    {
        // Disable all action maps first
        DisableGameplay();
        DisableUI();
        DisableCookingWithWords();
        DisableFlappyRhymes();

        // Proper cleanup (FIXES YOUR ERROR)
        if (inputActions != null)
        {
            inputActions.Disable();
            if (Application.isPlaying)
            {
                inputActions.Dispose();
            }
            inputActions = null;
        }
    }

    private void DisableGameplay()
    {
        if (inputActions != null)
            inputActions.Player.Disable();
    }

    private void DisableUI()
    {
        if (inputActions != null)
            inputActions.UI.Disable();
    }

    private void DisableCookingWithWords()
    {
        if (inputActions != null)
            inputActions.CookingWithWords.Disable();
    }

    private void DisableFlappyRhymes()
    {
        if (inputActions != null)
            inputActions.FlappyRhymes.Disable();
    }

    public void EnableGameplay()
    {
        inputActions.Player.Enable();
        inputActions.Player.SetCallbacks(this);

        DisableUI();
        DisableCookingWithWords();
        DisableFlappyRhymes();
    }

    public void EnableUI()
    {
        inputActions.UI.Enable();
        inputActions.UI.SetCallbacks(this);

        DisableGameplay();
        DisableCookingWithWords();
        DisableFlappyRhymes();
    }

    public void EnableCookingWithWords()
    {
        inputActions.CookingWithWords.Enable();
        inputActions.CookingWithWords.SetCallbacks(this);

        DisableUI();
        DisableGameplay();
        DisableFlappyRhymes();
    }

    public void EnableFlappyRhymes()
    {
        inputActions.FlappyRhymes.Enable();
        inputActions.FlappyRhymes.SetCallbacks(this);

        DisableUI();
        DisableGameplay();
        DisableCookingWithWords();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        // optional: implement or remove from interface if unused
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        touchEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started ||
            context.phase == InputActionPhase.Performed)
            touchPressEvent?.Invoke(true);

        if (context.phase == InputActionPhase.Canceled)
            touchPressEvent?.Invoke(false);
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            leftMouseButtonEvent?.Invoke();

        if (context.phase == InputActionPhase.Started)
            mousePressEvent?.Invoke(true);

        if (context.phase == InputActionPhase.Canceled)
        if (context.phase == InputActionPhase.Canceled)
            mousePressEvent?.Invoke(false);
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePosEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}