using UnityEngine;

public class ResetAction : StateMachineBehaviour
{
    PlayerController _controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_controller == null)
        {
            _controller = animator.GetComponent<PlayerController>();
        }

        _controller.IsPerformingAction = false;
        _controller.IsJumped = false;
        animator.applyRootMotion = false;
    }
}
