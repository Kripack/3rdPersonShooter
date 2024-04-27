using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManager")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    private GameInput gameInput;

    public bool aim;
    public bool sprint;
    public bool crouch;

    public Vector2 look;
    public Vector2 moveInput;
    public Vector2 previusMoveInput;

    public event Action Jump;
    public event Action Roll;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.UI.SetCallbacks(this);

            SetGameplay();
        }
    }
    private void SetGameplay()
    {
        gameInput.Gameplay.Enable();
        gameInput.UI.Disable();
    }
    private void SetUI()
    {
        gameInput.Gameplay.Disable();
        gameInput.UI.Enable();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValueAsButton();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            crouch = !crouch;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        SetUI();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump?.Invoke();
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Roll?.Invoke();
        }
    }
}
