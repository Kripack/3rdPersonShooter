using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MovementBaseState
{
    private float backSpeed;

    public RunningState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        controller.Animator.SetBool("Running", true);
        controller.SetSpeed(controller.sprintSpeed);
        backSpeed = controller.sprintSpeed / 2f;
    }

    public override void UpdateState()
    {
        if (controller.input.moveInput == Vector2.zero)
        {
            controller.SetMovementState(controller.IdleState);
        }
        else if (controller.input.sprint == false)
        {
            controller.SetMovementState(controller.WalkingState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(backSpeed);
        }
        else controller.SetSpeed(controller.sprintSpeed);
    }
    public override void ExitState()
    {
        controller.Animator.SetBool("Running", false);
    }
}
