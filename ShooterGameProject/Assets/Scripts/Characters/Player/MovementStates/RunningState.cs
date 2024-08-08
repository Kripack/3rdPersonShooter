using UnityEngine;

public class RunningState : MovementState
{
    private float _backSpeed;

    public RunningState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.RunningBool, true);
        controller.SetSpeed(controller.SprintSpeed);
        _backSpeed = controller.SprintSpeed / 2f;
        controller.CharacterAnimator.SetSpineAimWeight(controller.CharacterAnimator.SpineAimSprintWeight);
    }

    public override void Update()
    {
        if (controller.input.moveInput == Vector2.zero)
        {
            stateMachine.SetState(controller.IdleState);
        }
        else if (controller.input.sprint == false)
        {
            stateMachine.SetState(controller.WalkingState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(_backSpeed);
        }
        else controller.SetSpeed(controller.SprintSpeed);
    }
    public override void OnExit()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.RunningBool, false);
        controller.CharacterAnimator.SetSpineAimWeight(controller.CharacterAnimator.SpineAimWeight);
    }
}
