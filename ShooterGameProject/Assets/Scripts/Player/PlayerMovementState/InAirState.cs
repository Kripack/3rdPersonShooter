using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class InAirState : MovementBaseState
{
    public InAirState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        Controller.SetSpeed(Controller.AirSpeed);
    }

    public override void UpdateState()
    {
        if (!Controller.IsPerformingAction && !Controller.IsJumped)
        {
            if (Controller.CombatSystem.WeaponEquiped) Controller.CharacterAnimator.Animator.CrossFade(Controller.CharacterAnimator.FallingLoopLegsOnly, 0.1f);
            else Controller.CharacterAnimator.Animator.CrossFade(Controller.CharacterAnimator.FallingLoop, 0.1f);

        }
        
        if (Controller.Motor.IsGrounded())
        {
            if (Controller.CombatSystem.WeaponEquiped) Controller.CharacterAnimator.PlayTargetActionAnimation(Controller.CharacterAnimator.LandingLegsOnlyAnimation, true);
            else Controller.CharacterAnimator.PlayTargetActionAnimation(Controller.CharacterAnimator.LandingAnimation, true);
            
            Controller.SetMovementState(Controller.IdleState);
        }
    }

}
