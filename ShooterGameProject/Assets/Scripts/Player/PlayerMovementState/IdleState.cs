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
        Controller.SetSpeed(Controller.MoveSpeed);
    }

    public override void UpdateState()
    {
        if (Controller.IsPerformingAction) return;
        if (Controller.input.moveInput != Vector2.zero)
        {
            if(Controller.input.sprint == true)
            {
                Controller.SetMovementState(Controller.RunningState);
            }
            else Controller.SetMovementState(Controller.WalkingState);
        }
        if (Controller.input.crouch == true)
        {
            Controller.SetMovementState(Controller.CrouchingState);
        }
    }
}
