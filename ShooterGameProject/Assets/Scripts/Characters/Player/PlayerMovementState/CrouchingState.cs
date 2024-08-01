using UnityEngine;

public class CrouchingState : MovementState
{
    private float _backSpeed;

    public CrouchingState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.CrouchingBool, true);
        Controller.SetSpeed(Controller.CrouchSpeed);
        _backSpeed = Controller.CrouchSpeed / 2f;
    }

    public override void Update()
    {
        if (Controller.input.crouch == false)
        {
            if (Controller.input.moveInput == Vector2.zero)
            {
                StateMachine.SetState(Controller.IdleState);
            }
            else StateMachine.SetState(Controller.WalkingState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.CrouchSpeed);
    }
    public override void OnExit()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.CrouchingBool, false);
    }
}
