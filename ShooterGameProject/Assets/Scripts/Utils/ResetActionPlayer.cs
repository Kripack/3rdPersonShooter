using UnityEngine;

public class ResetActionPlayer : StateMachineBehaviour
{
    PlayerController _controller;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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
