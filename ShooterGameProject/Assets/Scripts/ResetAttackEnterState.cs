using UnityEngine;

public class ResetAttackEnterState : StateMachineBehaviour
{
    private CombatSystemController _combatSystemController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_combatSystemController == null)
        {
            _combatSystemController = animator.GetComponent<CombatSystemController>();
        }
        _combatSystemController.ResetAttack();
    }

}
