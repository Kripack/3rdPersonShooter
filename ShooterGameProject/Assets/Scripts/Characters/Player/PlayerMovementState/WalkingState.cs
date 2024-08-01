using UnityEngine;

public class WalkingState : MovementState
{
    private float _backSpeed;

    public WalkingState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.WalkingBool, true);
        Controller.SetSpeed(Controller.MoveSpeed);
        _backSpeed = Controller.MoveSpeed / 2f;
    }

    public override void Update()
    {
        if (Controller.input.sprint == true)
        {
            StateMachine.SetState(Controller.RunningState);
        }
        else if(Controller.input.crouch == true)
        {
            StateMachine.SetState(Controller.CrouchingState);
        }
        else if(Controller.input.moveInput == Vector2.zero)
        {
            StateMachine.SetState(Controller.IdleState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.MoveSpeed);
    }
    public override void OnExit()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.WalkingBool, false);
    }
}
