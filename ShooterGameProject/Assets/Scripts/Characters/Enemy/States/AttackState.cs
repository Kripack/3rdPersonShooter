using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyBaseState
{
    public AttackState(EnemyStateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
        : base(stateMachine, animator, agent, enemy, detector)
    {
    }

    public override void OnEnter()
    {
        animator.CrossFade(idleHash, crossFadeDuration);
        agent.updateRotation = false;
        agent.isStopped = true;
    }

    public override void Update()
    {
        agent.SetDestination(detector.Player.position);
        
        var direction = detector.Player.position - detector.Origin.position;
        direction.y = 0f;
        enemy.Locomotion.Rotate(direction, enemy.Data.rotationSpeed);
        
        if (!enemy.AttackTimer.IsRunning)
        {
            enemy.AttackTimer.Start();
            RandomAttack();
            return;
        }
        
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();

            if (!detector.CanAttackPlayer())
            {
                stateMachine.SetState(stateMachine.ChaseState); 
            }
        }
    }

    private void RandomAttack()
    {
        int index = Random.Range(1, 3);
        switch (index)
        {
            case 1:
                animator.CrossFade(attack01Hash, 0.2f);
                break;
            case 2:
                animator.CrossFade(attack02Hash, 0.2f);
                break;
        }
    }
    
    public override void OnExit()
    {
        agent.updateRotation = true;
        agent.isStopped = false;
    }
    
}