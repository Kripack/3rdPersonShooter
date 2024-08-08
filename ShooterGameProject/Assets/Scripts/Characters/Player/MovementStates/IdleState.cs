using UnityEngine;

public class IdleState : MovementState
{
    public IdleState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        controller.SetSpeed(controller.MoveSpeed);
    }

    public override void Update()
    {
        if (controller.IsPerformingAction) return;
        if (controller.input.moveInput != Vector2.zero)
        {
            if(controller.input.sprint == true)
            {
                stateMachine.SetState(controller.RunningState);
            }
            else stateMachine.SetState(controller.WalkingState);
        }
        if (controller.input.crouch == true)
        {
            stateMachine.SetState(controller.CrouchingState);
        }
    }
}
