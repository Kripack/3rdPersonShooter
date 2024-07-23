using UnityEngine;

public class ResetAttackEnterState : StateMachineBehaviour
{
    private CombatSystem _combatSystem;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_combatSystem == null)
        {
            _combatSystem = animator.GetComponent<CombatSystem>();
        }
        _combatSystem.ResetAttack();
    }

}
