using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    public IdleState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        controller.SetSpeed(controller.moveSpeed);
    }

    public override void UpdateState()
    {
        if (controller.isPerformingAction) return;
        if (controller.input.moveInput != Vector2.zero)
        {
            if(controller.input.sprint == true)
            {
                controller.SetMovementState(controller.RunningState);
            }
            else controller.SetMovementState(controller.WalkingState);
        }
        if (controller.input.crouch == true)
        {
            controller.SetMovementState(controller.CrouchingState);
        }
    }
}
