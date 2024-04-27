using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : MovementBaseState
{
    public InAirState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        controller.SetSpeed(controller.AirSpeed);
    }

    public override void UpdateState()
    {
        if (!controller.isPerformingAction) 
        {
            controller.CharacterAnimator.PlayTargetActionAnimation("FallingLoop", false);
            //controller.Animator.Play("FallingLoop",1,1f);
        }
        
        if (controller.Motor.IsGrounded())
        {
            controller.CharacterAnimator.PlayTargetActionAnimation("Landing", true);
            controller.SetMovementState(controller.IdleState);
        }
    }

}
