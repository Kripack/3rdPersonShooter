using UnityEngine;

public class WalkingState : PlayerBaseState
{
    private float _backSpeed;

    public WalkingState(PlayerStateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.WalkingBool, true);
        controller.SetSpeed(controller.MoveSpeed);
        _backSpeed = controller.MoveSpeed / 2f;
    }

    public override void Update()
    {
        if (controller.input.sprint == true)
        {
            stateMachine.SetState(stateMachine.RunningState);
        }
        else if(controller.input.crouch == true)
        {
            stateMachine.SetState(stateMachine.CrouchingState);
        }
        else if(controller.input.moveInput == Vector2.zero)
        {
            stateMachine.SetState(stateMachine.IdleState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(_backSpeed);
        }
        else controller.SetSpeed(controller.MoveSpeed);
    }
    public override void OnExit()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.WalkingBool, false);
    }
}
