using UnityEngine;

public class RunningState : MovementState
{
    private float _backSpeed;

    public RunningState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.RunningBool, true);
        Controller.SetSpeed(Controller.SprintSpeed);
        _backSpeed = Controller.SprintSpeed / 2f;
        Controller.CharacterAnimator.SetSpineAimWeight(Controller.CharacterAnimator.SpineAimSprintWeight);
    }

    public override void Update()
    {
        if (Controller.input.moveInput == Vector2.zero)
        {
            StateMachine.SetState(Controller.IdleState);
        }
        else if (Controller.input.sprint == false)
        {
            StateMachine.SetState(Controller.WalkingState);
        }

        if (Controller.input.moveInput.y < 0)
        {
            Controller.SetSpeed(_backSpeed);
        }
        else Controller.SetSpeed(Controller.SprintSpeed);
    }
    public override void OnExit()
    {
        Controller.CharacterAnimator.Animator.SetBool(Controller.CharacterAnimator.RunningBool, false);
        Controller.CharacterAnimator.SetSpineAimWeight(Controller.CharacterAnimator.SpineAimWeight);
    }
}
