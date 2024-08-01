using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState : BaseState
{
    protected Animator Animator;
    protected NavMeshAgent Agent;
    protected Enemy EnemyCharacter;
    
    protected readonly int IdleHash = Animator.StringToHash("IdleNormal");
    protected readonly int RunHash = Animator.StringToHash("RunFWD");
    protected readonly int WalkHash = Animator.StringToHash("WalkFWD");
    protected readonly int AttackHash = Animator.StringToHash("Attack01");
    protected readonly int DieHash = Animator.StringToHash("Die");
    
    protected const float CrossFadeDuration = 0.1f;
    
    public EnemyBaseState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy) : base(stateMachine)
    {
        Animator = animator;
        Agent = agent;
        EnemyCharacter = enemy;
    }
}
