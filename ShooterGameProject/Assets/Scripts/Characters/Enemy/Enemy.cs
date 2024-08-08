using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Hitbox))]
public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public EnemyData Data { get; private set; }
    [SerializeField] private LayerMask attackMask;
    public CountdownTimer AttackTimer { get; private set; }
    
    #region State Machine

    public EnemyWanderState WanderState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    
    private StateMachine _stateMachine;

    #endregion

    public BaseLocomotion _locomotion;
    private PlayerDetector _detector;
    private IAttackStrategy _attackStrategy;
    private Health _health;
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _died;
    private int _dieHash;
    
    private void Awake()
    {
        _stateMachine = new StateMachine();
        _health = new Health(Data.maxHp);
        _detector = new PlayerDetector(GameObject.FindGameObjectWithTag("Player").transform, transform, Data);
        AttackTimer = new CountdownTimer(Data.attackRate);
        _attackStrategy = new SphereCastStrategy(attackMask, Data.attackArea);
        _locomotion = new BaseLocomotion();
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.speed = Data.speed;
        _agent.stoppingDistance = Data.attackRange - 0.25f;
        _animator = GetComponentInChildren<Animator>();
        _dieHash = Animator.StringToHash("Die");
        
        WanderState = new EnemyWanderState(_stateMachine, _animator, _agent, this, _detector);
        ChaseState = new EnemyChaseState(_stateMachine, _animator, _agent, this, _detector);
        AttackState = new EnemyAttackState(_stateMachine, _animator, _agent, this, _detector);
        _stateMachine.SetState(WanderState);
    }

    private void Update()
    {
        AttackTimer.Tick(Time.deltaTime);
        if(!_died) _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _locomotion.Move(transform, _agent.desiredVelocity);
    }

    public void Attack()
    {
        var hitCollider = _attackStrategy.Execute(transform.position + Vector3.up, transform.forward, Data.attackRange);
        if (hitCollider?.TryGetComponent<IDamageable>(out var damageable) == true)
        {
            damageable.TakeDamage(Data.damage);
            Debug.Log(transform.name + " deal " + Data.damage + " to Player!");
        }
        
    }
    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }

    private void Die()
    {
        _animator.Play(_dieHash, 0 , 0.1f);
        
        GetComponent<Collider>().enabled = false; // ?
        _died = true;
        _agent.isStopped = true;
    }

    private void OnEnable()
    {
        _health.Die += Die;
    }

    private void OnDisable()
    {
        _health.Die -= Die;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.attackArea);

        if (Physics.SphereCast(transform.position + Vector3.up, Data.attackArea, transform.forward, out var hit, Data.attackRange, attackMask))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hit.point, Data.attackArea);
            Gizmos.DrawLine(transform.position, hit.point);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * Data.attackRange);
        }
    }
    
}
