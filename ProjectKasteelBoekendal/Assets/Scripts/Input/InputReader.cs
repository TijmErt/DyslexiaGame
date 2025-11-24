using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, InputActions.IPlayerActions, InputActions.IUIActions, InputActions.ICookingWithWordsActions
{
    public InputActions inputActions { get; private set; }

    public event UnityAction<Vector2> touchEvent;
    public event UnityAction leftMouseButtonEvent;
    public event UnityAction<Vector2> mousePosEvent;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new();
        }
        EnableCookingWithWords();
    }

    private void OnDisable()
    {
        DisableCookingWithWords();
    }

    private void DisableGameplay() => inputActions.Player.Disable();
    private void DisableUI() => inputActions.UI.Disable();
    private void DisableCookingWithWords() => inputActions.CookingWithWords.Disable();

    public void EnableGameplay()
    {
        inputActions.Player.Enable();
        inputActions.Player.SetCallbacks(this);
        DisableUI();
        DisableCookingWithWords();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void EnableCookingWithWords()
    {
        inputActions.CookingWithWords.Enable();
        inputActions.CookingWithWords.SetCallbacks(this);
        DisableUI();
        DisableGameplay();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        touchEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            leftMouseButtonEvent?.Invoke();
        }
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        mousePosEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
