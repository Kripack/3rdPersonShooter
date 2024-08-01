using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : EnemyBaseState 
{
    private readonly Vector3 _startPoint;
    private readonly float _wanderRadius;

    public EnemyWanderState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, float wanderRadius) : base(
            stateMachine, animator, agent, enemy)
    {
        _startPoint = enemy.transform.position;
        _wanderRadius = wanderRadius;
    }
        
    public override void OnEnter() 
    {
        Debug.Log("Wander"); 
        Animator.CrossFade(WalkHash, CrossFadeDuration);
    }

    public override void Update() {
        if (HasReachedDestination()) {
            var randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection += _startPoint;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _wanderRadius, 1);
            var finalPosition = hit.position;
                
            Agent.SetDestination(finalPosition);
        }
    }
        
    bool HasReachedDestination() {
        return !Agent.pathPending
               && Agent.remainingDistance <= Agent.stoppingDistance
               && (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f);
    }
}