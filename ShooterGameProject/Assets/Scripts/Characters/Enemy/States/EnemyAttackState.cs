using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
        : base(stateMachine, animator, agent, enemy, detector)
    {
    }

    public override void OnEnter()
    {
        animator.CrossFade(idleHash, crossFadeDuration);
    }

    public override void Update()
    {
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();

            if (!detector.CanAttackPlayer())
            {
                stateMachine.SetState(enemy.ChaseState); 
                Debug.Log("To chasing state");
                return;
            }
        }
        
        agent.SetDestination(detector.Player.position);
        if (!enemy.AttackTimer.IsRunning)
        {
            Vector3 direction = detector.Player.position - detector.Origin.position;
            direction.y = 0f;
            //enemy.Rotate(direction);
            enemy.StartCoroutine(enemy._locomotion.RotateTowards(detector.Origin, direction, enemy.Data.rotationSpeed));
            
            enemy.AttackTimer.Start();
            animator.Play(attackHash);
        }
    }
}