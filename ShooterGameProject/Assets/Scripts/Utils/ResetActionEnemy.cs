using UnityEngine;

public class ResetActionEnemy : StateMachineBehaviour
{
    private Enemy _enemy;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_enemy == null)
        {
            _enemy = animator.GetComponent<Enemy>();
        }

        _enemy.Agent.speed = _enemy.Data.speed;
    }
}