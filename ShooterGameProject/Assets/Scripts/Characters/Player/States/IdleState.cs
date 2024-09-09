using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
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
                stateMachine.SetState(stateMachine.RunningState);
            }
            else stateMachine.SetState(stateMachine.WalkingState);
        }
        if (controller.input.crouch == true)
        {
            stateMachine.SetState(stateMachine.CrouchingState);
        }
    }
}
