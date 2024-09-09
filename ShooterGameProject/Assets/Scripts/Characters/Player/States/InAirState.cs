public class InAirState : PlayerBaseState
{
    public InAirState(PlayerStateMachine stateMachine, PlayerController controller) : base(stateMachine, controller)
    {
    }

    public override void OnEnter()
    {
        controller.SetSpeed(controller.AirSpeed);
    }

    public override void Update()
    {
        if (!controller.IsPerformingAction && !controller.IsJumped)
        {
            if (controller.CombatSystemController.WeaponEquiped) controller.CharacterAnimator.Animator.CrossFade(controller.CharacterAnimator.FallingLoopLegsOnly, 0.1f);
            else controller.CharacterAnimator.Animator.CrossFade(controller.CharacterAnimator.FallingLoop, 0.1f);

        }
        
        if (controller.Motor.IsGrounded())
        {
            if (controller.CombatSystemController.WeaponEquiped) controller.CharacterAnimator.PlayTargetActionAnimation(controller.CharacterAnimator.LandingLegsOnlyAnimation, true);
            else controller.CharacterAnimator.PlayTargetActionAnimation(controller.CharacterAnimator.LandingAnimation, true);
            
            stateMachine.SetState(stateMachine.IdleState);
        }
    }

}
