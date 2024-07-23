using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovementBaseState
{
    private float _backSpeed;

    public WalkingState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.WalkingBool, true);
        Controller.SetSpeed(Controller.MoveSpeed);
        _backSpeed = Controller.MoveSpeed / 2f;
    }

    public override void UpdateState()
    {
        if (Controller.input.sprint == true)
        {
           Controller.SetMovementState(Controller.RunningState);
        }
        else if(Controller.input.crouch == true)
        {
            Controller.SetMovementState(Controller.CrouchingState);
        }
        else if(Controller.input.moveInput == Vector2.zero)
        {
           Controller.SetMovementState(Controller.IdleState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.MoveSpeed);
    }
    public override void ExitState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.WalkingBool, false);
    }
}
