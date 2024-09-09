using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleStateEnemy : EnemyBaseState
{
    private readonly CountdownTimer _idleTimer;
    public IdleStateEnemy(EnemyStateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector) : base(stateMachine, animator, agent, enemy, detector)
    {
        _idleTimer = new CountdownTimer(enemy.Data.idleTime);
    }
    public override void OnEnter()
    {
        animator.CrossFade(idleHash, crossFadeDuration);
        _idleTimer.Start();
    }

    public override void Update()
    {
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();

            if (detector.CanDetectPlayer())
            {
                stateMachine.SetState(stateMachine.ChaseState); 
            }
        }
        
        _idleTimer.Tick(Time.deltaTime);
        if (!_idleTimer.IsRunning)
        {
            stateMachine.SetState(stateMachine.WanderState);
        }
    }
}
