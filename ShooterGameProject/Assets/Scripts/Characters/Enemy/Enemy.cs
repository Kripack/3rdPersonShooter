using UnityEngine;
using UnityEngine.AI;

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
    
    #region State Machine

    public WanderState WanderState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    
    private StateMachine _stateMachine;

    #endregion
    
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
        _stateMachine = new StateMachine();
        _health = new Health(Data.maxHp);
        _detector = new PlayerDetector(GameObject.FindGameObjectWithTag("Player").transform, transform, Data);
        AttackTimer = new CountdownTimer(Data.attackRate);
        _attackStrategy = new SphereCastStrategy(attackMask, Data.attackArea);
        Locomotion = new BaseLocomotion(transform);
        _hitbox = GetComponent<Hitbox>();
        _headHitbox = GetComponentInChildren<HeadHitbox>();
        _animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _ownCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        Agent.updatePosition = false;
        Agent.speed = Data.speed;
        Agent.stoppingDistance = Data.attackRange - 0.5f;
        _dieHash = Animator.StringToHash("Die");
        _hitHash = Animator.StringToHash("GetHit");
        _hitbox.SetBodyType(Data.bodyType);
        
        WanderState = new WanderState(_stateMachine, _animator, Agent, this, _detector);
        ChaseState = new ChaseState(_stateMachine, _animator, Agent, this, _detector);
        AttackState = new AttackState(_stateMachine, _animator, Agent, this, _detector);
        _stateMachine.SetState(WanderState);
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
            AcceptAttack(attackVisitor, hit);
            SoundFXManager.instance.PlayRandomAudioClip(Data.audioFXPreset.attackClips, _audioSource, 0.75f);
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
        Agent.speed /= 3f;
        ChaseState.AgroStatus();
        if (_stateMachine.CurrentState != ChaseState)  _stateMachine.SetState(ChaseState);
        _animator.CrossFade(_hitHash, 0.1f);
    }
    
    private void Die()
    {
        Agent.baseOffset = 0f;
        _ownCollider.enabled = false;
        IsDead = true;
        Agent.isStopped = true;
        
        _animator.CrossFade(_dieHash, 0.1f);
    }

    private void OnEnable()
    {
        _health.Die += Die;
        _hitbox.OnHit += OnHit;
        if (_headHitbox != null) _headHitbox.OnHit += OnHit;
    }

    private void OnDisable()
    {
        _health.Die -= Die;
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
