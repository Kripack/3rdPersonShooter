using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAction : StateMachineBehaviour
{
    PlayerController controller;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller == null)
        {
            controller = animator.GetComponentInParent<PlayerController>();
        }

        controller.isPerformingAction = false;
        animator.applyRootMotion = false;
    }
}
