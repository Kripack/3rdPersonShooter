using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public EnemyData Data { get; private set; }
    [SerializeField] private LayerMask attackMask;
    public CountdownTimer AttackTimer { get; private set; }
    public BaseLocomotion Locomotion { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public bool IsDead { get; set; }
    
    [Inject] private EnemyList _enemyList;
    [Inject] private PlayerController _player;
    private EnemyStateMachine _stateMachine;
    private Hitbox _hitbox;
    private HeadHitbox _headHitbox;
    private PlayerDetector _detector;
    private IAttackStrategy _attackStrategy;
    private Health _health;
    private Animator _animator;
    private AudioSource _audioSource;
    private int _dieHash;
    private int _hitHash;
    private Collider _ownCollider;

    private void Awake()
    {
        _stateMachine = new EnemyStateMachine();
        _health = new Health(Data.maxHp);
        _detector = new PlayerDetector(_player.transform, transform, Data);
        AttackTimer = new CountdownTimer(Data.attackRate);
        _attackStrategy = new SphereCastStrategy(attackMask, Data.attackArea);
        Locomotion = new BaseLocomotion(transform);
        _hitbox = GetComponent<Hitbox>();
        _headHitbox = GetComponentInChildren<HeadHitbox>();
        _animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _ownCollider = GetComponent<Collider>();
        _stateMachine.InitStates(this, _animator, Agent, _detector);
    }

    private void Start()
    {
        Agent.updatePosition = false;
        Agent.speed = Data.speed;
        Agent.stoppingDistance = Data.attackRange - 0.5f;
        _dieHash = Animator.StringToHash("Die");
        _hitHash = Animator.StringToHash("GetHit");
        _hitbox.SetBodyType(Data.bodyType);
        
        _stateMachine.SetState(_stateMachine.WanderState);
        
        _enemyList.RegisterEnemy(this);
    }

    private void Update()
    {
        AttackTimer.Tick(Time.deltaTime);
        if(!IsDead) _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        Locomotion.MoveTo(Agent.nextPosition);
    }

    public void Attack()
    {
        var hit = _attackStrategy.Execute(transform.position + Vector3.up, transform.forward, Data.attackRange);
        if (hit.collider == null) return;
        if (hit.collider.TryGetComponent<IAttackVisitor>(out var attackVisitor) == true)
        {
            AcceptAttack(attackVisitor, hit, 1f);
        }
    }

    public void SecondAttack()
    {
        var hit = _attackStrategy.Execute(transform.position + Vector3.up, transform.forward, Data.attackRange);
        if (hit.collider == null) return;
        if (hit.collider.TryGetComponent<IAttackVisitor>(out var attackVisitor) == true)
        {
            AcceptAttack(attackVisitor, hit, Data.secondAttackMultiplier);
        }
    }
    
    private void AcceptAttack(IAttackVisitor attackVisitor, RaycastHit hit, float damageMultiplier)
    {
        attackVisitor.Visit(this, hit, damageMultiplier);
    }
    
    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }
    
    private void OnHit()
    {
        _stateMachine.ChaseState.AgroStatus();
        if (_stateMachine.CurrentState != _stateMachine.ChaseState)  _stateMachine.SetState(_stateMachine.ChaseState);
        Agent.speed /= 3f;
        _animator.CrossFade(_hitHash, 0.1f);
    }
    
    private void OnDeath()
    {
        Agent.baseOffset = 0f;
        _ownCollider.enabled = false;
        IsDead = true;
        Agent.isStopped = true;
        Agent.radius = 0f;
        
        _animator.CrossFade(_dieHash, 0.1f);
        
        _enemyList.EnemyDefeated(this);
    }

    private void OnEnable()
    {
        _health.Die += OnDeath;
        _hitbox.OnHit += OnHit;
        if (_headHitbox != null) _headHitbox.OnHit += OnHit;
    }

    private void OnDisable()
    {
        _health.Die -= OnDeath;
        _hitbox.OnHit -= OnHit;
        if (_headHitbox != null) _headHitbox.OnHit -= OnHit;
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
