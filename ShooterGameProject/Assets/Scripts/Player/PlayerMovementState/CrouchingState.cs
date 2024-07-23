using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : MovementBaseState
{
    private float _backSpeed;

    public CrouchingState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.CrouchingBool, true);
        Controller.SetSpeed(Controller.CrouchSpeed);
        _backSpeed = Controller.CrouchSpeed / 2f;
    }

    public override void UpdateState()
    {
        if (Controller.input.crouch == false)
        {
            if (Controller.input.moveInput == Vector2.zero)
            {
                Controller.SetMovementState(Controller.IdleState);
            }
            else Controller.SetMovementState(Controller.WalkingState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.CrouchSpeed);
    }
    public override void ExitState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.CrouchingBool, false);
    }
}
