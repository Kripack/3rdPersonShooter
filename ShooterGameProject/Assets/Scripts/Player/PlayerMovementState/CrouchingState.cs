using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : MovementBaseState
{
    private float backSpeed;

    public CrouchingState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        controller.Animator.SetBool("Crouching", true);
        controller.SetSpeed(controller.crouchSpeed);
        backSpeed = controller.crouchSpeed / 2f;
    }

    public override void UpdateState()
    {
        if (controller.input.crouch == false)
        {
            if (controller.input.moveInput == Vector2.zero)
            {
                controller.SetMovementState(controller.IdleState);
            }
            else controller.SetMovementState(controller.WalkingState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(backSpeed);
        }
        else controller.SetSpeed(controller.crouchSpeed);
    }
    public override void ExitState()
    {
        controller.Animator.SetBool("Crouching", false);
    }
}
