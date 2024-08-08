using UnityEngine;

public class CrouchingState : MovementState
{
    private float _backSpeed;

    public CrouchingState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.CrouchingBool, true);
        controller.SetSpeed(controller.CrouchSpeed);
        _backSpeed = controller.CrouchSpeed / 2f;
    }

    public override void Update()
    {
        if (controller.input.crouch == false)
        {
            if (controller.input.moveInput == Vector2.zero)
            {
                stateMachine.SetState(controller.IdleState);
            }
            else stateMachine.SetState(controller.WalkingState);
        }

        if (controller.input.moveInput.y < 0)
        {
            controller.SetSpeed(_backSpeed);
        }
        else controller.SetSpeed(controller.CrouchSpeed);
    }
    public override void OnExit()
    {
        controller.CharacterAnimator.Animator.SetBool(controller.CharacterAnimator.CrouchingBool, false);
    }
}
