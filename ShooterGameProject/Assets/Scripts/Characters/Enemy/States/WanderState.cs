using UnityEngine;
using UnityEngine.AI;

public class WanderState : EnemyBaseState 
{
    private readonly Vector3 _startPoint;
    private readonly float _wanderRadius;
    private readonly float _wanderSpeed;

    public WanderState(EnemyStateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector) : base(
            stateMachine, animator, agent, enemy, detector)
    {
        _startPoint = detector.Origin.position;
        _wanderRadius = enemy.Data.wanderRadius;
        _wanderSpeed = enemy.Data.WanderSpeed;
    }
        
    public override void OnEnter() 
    {
        animator.CrossFade(walkHash, crossFadeDuration);
        
        agent.speed = _wanderSpeed;
        SetRandomDestination();
    }
    
    public override void Update() 
    {
        if (HasReachedDestination())
        {
            stateMachine.SetState(stateMachine.IdleStateEnemy);
        }
        
        detectionTimer.Tick(Time.deltaTime);
        if (!detectionTimer.IsRunning)
        {
            detectionTimer.Start();

            if (detector.CanDetectPlayer())
            {
                stateMachine.SetState(stateMachine.ChaseState); 
            }
        }
    }

    private void SetRandomDestination()
    {
        var randomDirection = Random.insideUnitSphere * _wanderRadius;
        randomDirection += _startPoint;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, _wanderRadius, 1);
        var finalPosition = hit.position;
                
        agent.SetDestination(finalPosition);
    }

    private bool HasReachedDestination() 
    {
        return !agent.pathPending
               && agent.remainingDistance <= agent.stoppingDistance
               && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}