using UnityEngine;

public class IdleState : MovementState
{
    public IdleState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        Controller.SetSpeed(Controller.MoveSpeed);
    }

    public override void Update()
    {
        if (Controller.IsPerformingAction) return;
        if (Controller.input.moveInput != Vector2.zero)
        {
            if(Controller.input.sprint == true)
            {
                StateMachine.SetState(Controller.RunningState);
            }
            else StateMachine.SetState(Controller.WalkingState);
        }
        if (Controller.input.crouch == true)
        {
            StateMachine.SetState(Controller.CrouchingState);
        }
    }
}
