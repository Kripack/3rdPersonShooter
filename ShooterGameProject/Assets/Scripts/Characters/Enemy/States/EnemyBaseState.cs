using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState : BaseState
{
    protected EnemyStateMachine stateMachine;
    protected readonly Animator animator;
    protected readonly NavMeshAgent agent;
    protected readonly Enemy enemy;
    protected readonly PlayerDetector detector;
    protected readonly CountdownTimer detectionTimer;
    
    protected readonly int idleHash = Animator.StringToHash("IdleNormal");
    protected readonly int runHash = Animator.StringToHash("RunFWD");
    protected readonly int walkHash = Animator.StringToHash("WalkFWD");
    protected readonly int attackHash = Animator.StringToHash("Attack01");
    protected readonly int dieHash = Animator.StringToHash("Die");
    
    protected const float crossFadeDuration = 0.2f;
    
    public EnemyBaseState(EnemyStateMachine enemyStateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
    {
        this.stateMachine = enemyStateMachine;
        this.animator = animator;
        this.agent = agent;
        detectionTimer = new CountdownTimer(enemy.Data.detectionCooldown);
        this.detector = detector;
        this.enemy = enemy;
    }
}
