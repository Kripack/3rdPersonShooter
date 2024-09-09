using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    public WanderState WanderState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public IdleStateEnemy IdleStateEnemy { get; private set; }

    public void InitStates(Enemy enemy, Animator animator, NavMeshAgent agent, PlayerDetector detector)
    {
        WanderState = new WanderState(this, animator, agent, enemy, detector);
        ChaseState = new ChaseState(this, animator, agent, enemy, detector);
        AttackState = new AttackState(this, animator, agent, enemy, detector);
        IdleStateEnemy = new IdleStateEnemy(this, animator, agent, enemy, detector);
    }
}