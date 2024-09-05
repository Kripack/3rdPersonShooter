using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyBaseState
{
    private readonly float _chaseSpeed;
    private bool _agred;
    private bool _turned;

    public ChaseState(StateMachine stateMachine, Animator animator, NavMeshAgent agent, Enemy enemy, PlayerDetector detector)
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
        if (!_turned)
        {
            agent.SetDestination(detector.Origin.position);
            
            agent.updateRotation = false;
             var direction = detector.Player.position - detector.Origin.position;
             enemy.Locomotion.Rotate(direction, enemy.Data.rotationSpeed);

            if (detector.DoesDirectionMatch(5f))
            {            
                agent.updateRotation = true;
                _turned = true;
            }
        }
        else
        {
            agent.SetDestination(detector.Player.position);
        }
        
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

    public override void OnExit()
    {
        _turned = false;
    }

    public void AgroStatus()
    {
        _agred = true;
    }
}
