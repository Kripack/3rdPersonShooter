using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState : BaseState
{
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
    
    public EnemyBaseState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
        : base(stateMachine)
    {
        this.animator = animator;
        this.agent = agent;
        detectionTimer = new CountdownTimer(enemy.Data.detectionCooldown);
        this.detector = detector;
        this.enemy = enemy;
    }
}
