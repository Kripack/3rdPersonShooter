using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    private GameInput _gameInput;

    public bool aim;
    public bool sprint;
    public bool crouch;

    public Vector2 look;
    public Vector2 moveInput;

    #region Events
    public event Action Jump;
    public event Action Roll;
    public event Action Aim;
    public event Action ViewChange;
    public event Action FreeLook;
    public event Action SelectWeapon;
    public event Action Attack;
    public event Action StartAutoAttack;
    public event Action StopAutoAttack;
    public event Action Reload;
    #endregion

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new();
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);

            SetGameplay();
        }
    }
    private void SetGameplay()
    {
        _gameInput.Gameplay.Enable();
        _gameInput.UI.Disable();
    }
    private void SetUI()
    {
        _gameInput.Gameplay.Disable();
        _gameInput.UI.Enable();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValueAsButton();
        Aim?.Invoke();
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
        if (context.phase is InputActionPhase.Started)
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
        if (context.phase is InputActionPhase.Started)
        {
            Jump?.Invoke();
        }
    }

    public void OnRolling(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
        {
            Roll?.Invoke();
        }
    }

    public void OnChangeView(InputAction.CallbackContext context)
    { 
        if (context.phase is InputActionPhase.Started)
        {
            if (context.interaction is HoldInteraction) FreeLook?.Invoke();
        }
        if (context.phase is InputActionPhase.Performed)
        {
            if (context.interaction is TapInteraction) ViewChange?.Invoke();
        }
        if (context.phase is InputActionPhase.Canceled)
        {
            if (context.interaction is HoldInteraction) FreeLook?.Invoke();
        }
    }

    public void OnSelectWeapon(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Performed)
        {
            SelectWeapon?.Invoke();
        }
    }

    public void OnAttack_1(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
        {
            Attack?.Invoke();
            if (context.interaction is HoldInteraction) StartAutoAttack?.Invoke();
        }
        if (context.phase is InputActionPhase.Canceled)
        {
            if (context.interaction is HoldInteraction) StopAutoAttack?.Invoke();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase is InputActionPhase.Started)
        {
            Reload?.Invoke();
        }
    }
}
