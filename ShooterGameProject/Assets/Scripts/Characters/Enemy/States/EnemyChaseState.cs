using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private readonly float _chaseSpeed;
    private bool _agred;

    public EnemyChaseState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
        : base(stateMachine, animator, agent, enemy, detector)
    {
        _chaseSpeed = enemy.Data.ChaseSpeed;
    }

    public override void OnEnter()
    {
        if(!_agred) animator.CrossFade(runHash, crossFadeDuration);
        agent.speed = _chaseSpeed;
    }

    public override void Update()
    {
        agent.SetDestination(detector.Player.position);
        
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();
            
            if (detector.CanAttackPlayer())
            {
                _agred = false;
                stateMachine.SetState(enemy.AttackState);
                return;
            }

            if (!_agred)
            {
                if (!detector.CanDetectPlayer())
                {
                    stateMachine.SetState(enemy.WanderState);
                }
            }
        }
    }

    public void AgroStatus()
    {
        _agred = true;
    }
}
