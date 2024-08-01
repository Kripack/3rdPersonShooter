public class InAirState : MovementState
{
    public InAirState(StateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        Controller.SetSpeed(Controller.AirSpeed);
    }

    public override void Update()
    {
        if (!Controller.IsPerformingAction && !Controller.IsJumped)
        {
            if (Controller.CombatSystemController.WeaponEquiped) Controller.CharacterAnimator.Animator.CrossFade(Controller.CharacterAnimator.FallingLoopLegsOnly, 0.1f);
            else Controller.CharacterAnimator.Animator.CrossFade(Controller.CharacterAnimator.FallingLoop, 0.1f);

        }
        
        if (Controller.Motor.IsGrounded())
        {
            if (Controller.CombatSystemController.WeaponEquiped) Controller.CharacterAnimator.PlayTargetActionAnimation(Controller.CharacterAnimator.LandingLegsOnlyAnimation, true);
            else Controller.CharacterAnimator.PlayTargetActionAnimation(Controller.CharacterAnimator.LandingAnimation, true);
            
            StateMachine.SetState(Controller.IdleState);
        }
    }

}
