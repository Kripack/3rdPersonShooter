using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MovementBaseState
{
    public JumpState(PlayerController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        Controller.SetMovementState(Controller.IdleState);
    }
}
