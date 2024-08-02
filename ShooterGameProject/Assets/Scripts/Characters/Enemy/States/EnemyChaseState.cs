using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private readonly float _chaseSpeed;
    public EnemyChaseState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
        : base(stateMachine, animator, agent, enemy, detector)
    {
        _chaseSpeed = enemy.Data.ChaseSpeed;
    }

    public override void OnEnter()
    {
        animator.CrossFade(runHash, crossFadeDuration);
        agent.speed = _chaseSpeed;
        Debug.Log("Chase state!");
    }

    public override void Update()
    {
        agent.SetDestination(detector.Player.position);
        
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();

            if (!detector.CanDetectPlayer())
            {
                stateMachine.SetState(enemy.WanderState);
                Debug.Log("To wander state");
                return;
            }

            if (detector.CanAttackPlayer())
            {
                stateMachine.SetState(enemy.AttackState);
                Debug.Log("To attack state");
            }
        }
    }
}
