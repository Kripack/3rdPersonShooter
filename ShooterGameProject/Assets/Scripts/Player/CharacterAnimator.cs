using UnityEngine;

public class CharacterAnimator
{
    private PlayerController _controller;
    public CharacterAnimator(PlayerController controller)
    {
        _controller = controller;
    }
    public void UpdateAnimationBlend(Vector2 moveInput)
    {
        _controller.Animator.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
        _controller.Animator.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
    }

    public void PlayTargetActionAnimation(string targerAnimation, bool isPerformingAction, bool applyRootMotion = false)
    {
        if (_controller.isPerformingAction) return;
        _controller.Animator.applyRootMotion = applyRootMotion;
        _controller.isPerformingAction = isPerformingAction;
        _controller.Animator.CrossFade(targerAnimation, 0.2f);
    }
}
