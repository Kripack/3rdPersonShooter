using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovementBaseState
{
    private float _backSpeed;

    public RunningState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.RunningBool, true);
        Controller.SetSpeed(Controller.SprintSpeed);
        _backSpeed = Controller.SprintSpeed / 2f;
        Controller.CharacterAnimator.SetSpineAimWeight(Controller.CharacterAnimator.SpineAimSprintWeight);
    }

    public override void UpdateState()
    {
        if (Controller.input.moveInput == Vector2.zero)
        {
            Controller.SetMovementState(Controller.IdleState);
        }
        else if (Controller.input.sprint == false)
        {
            Controller.SetMovementState(Controller.WalkingState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.SprintSpeed);
    }
    public override void ExitState()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.RunningBool, false);
        Controller.CharacterAnimator.SetSpineAimWeight(Controller.CharacterAnimator.SpineAimWeight);
    }
}
