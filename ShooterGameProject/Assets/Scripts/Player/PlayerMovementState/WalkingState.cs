using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MovementBaseState
{
    private float backSpeed;

    public WalkingState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        controller.Animator.SetBool("Walking", true);
        controller.SetSpeed(controller.moveSpeed);
        backSpeed = controller.moveSpeed / 2f;
    }

    public override void UpdateState()
    {
        if (controller.input.sprint == true)
        {
           controller.SetMovementState(controller.RunningState);
        }
        else if(controller.input.crouch == true)
        {
            controller.SetMovementState(controller.CrouchingState);
        }
        else if(controller.input.moveInput == Vector2.zero)
        {
           controller.SetMovementState(controller.IdleState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(backSpeed);
        }
        else controller.SetSpeed(controller.moveSpeed);
    }
    public override void ExitState()
    {
        controller.Animator.SetBool("Walking", false);
    }
}
