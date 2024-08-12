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
    public BaseLocomotion Locomotion { get; private set; }
    
    #region State Machine

    public EnemyWanderState WanderState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    
    private StateMachine _stateMachine;

    #endregion
    
    private PlayerDetector _detector;
    private IAttackStrategy _attackStrategy;
    private Health _health;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Hitbox _hitbox;
    private bool _died;
    private int _dieHash;
    private int _hitHash;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _health = new Health(Data.maxHp);
        _detector = new PlayerDetector(GameObject.FindGameObjectWithTag("Player").transform, transform, Data);
        AttackTimer = new CountdownTimer(Data.attackRate);
        _attackStrategy = new SphereCastStrategy(attackMask, Data.attackArea);
        Locomotion = new BaseLocomotion(transform);
        _hitbox = GetComponent<Hitbox>();
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.updatePosition = false;
        _agent.speed = Data.speed;
        _agent.stoppingDistance = Data.attackRange - 0.5f;
        _dieHash = Animator.StringToHash("Die");
        _hitHash = Animator.StringToHash("GetHit");
        _hitbox.SetBodyType(Data.bodyType);
        
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
        Locomotion.MoveTo(_agent.nextPosition);
    }

    public void Attack()
    {
        var hit = _attackStrategy.Execute(transform.position + Vector3.up, transform.forward, Data.attackRange);
        if (hit.collider == null) return;
        if (hit.collider.TryGetComponent<IAttackVisitor>(out var attackVisitor) == true)
        {
            AcceptAttack(attackVisitor, hit);
            Debug.Log(transform.name + " deal " + Data.damage + " to Player!");
        }
        
    }

    private void AcceptAttack(IAttackVisitor attackVisitor, RaycastHit hit)
    {
        attackVisitor.Visit(this, hit);
    }
    
    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }

    private void OnHit()
    {
        _animator.Play(_hitHash, 0 , 0.1f);
    }
    
    private void Die()
    {
        _animator.Play(_dieHash, 0 , 0.1f);

        _agent.baseOffset = 0f;
        GetComponent<Collider>().enabled = false; // ?
        _died = true;
        _agent.isStopped = true;
    }

    private void OnEnable()
    {
        _health.Die += Die;
        _hitbox.OnHit += OnHit;
    }

    private void OnDisable()
    {
        _health.Die -= Die;
        _hitbox.OnHit -= OnHit;
    }
    
    private void OnDrawGizmosSelected()
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
