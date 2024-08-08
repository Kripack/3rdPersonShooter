using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Hitbox))]
public class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public EnemyData Data { get; private set; }
    public CountdownTimer AttackTimer { get; private set; }
    
    #region State Machine

    public EnemyWanderState WanderState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    
    private StateMachine _stateMachine;

    #endregion    

    private PlayerDetector _detector;
    private Health _health;
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _died;
    
    private void Awake()
    {
        _stateMachine = new StateMachine();
        _health = new Health(Data.maxHp);
        _detector = new PlayerDetector(GameObject.FindGameObjectWithTag("Player").transform, transform, Data);
        AttackTimer = new CountdownTimer(Data.attackRate);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.speed = Data.speed;
        _agent.stoppingDistance = Data.attackRange - 0.25f;
        _animator = GetComponentInChildren<Animator>();
        
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
        Move(_agent.desiredVelocity);
    }

    public void Attack()
    {
        Debug.Log("Attack Player!");
        _detector.Player.GetComponent<PlayerController>().TakeDamage(Data.damage);
    }
    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }
    private void Move(Vector3 desiredVelocity)
    {
        transform.position += desiredVelocity * Time.deltaTime;
    }
    private void Die()
    {
        int dieHash = Animator.StringToHash("Die");
        _animator.Play(dieHash, 0 , 0.1f);

        _died = true;
        _agent.isStopped = true;
        Debug.Log(transform.name + "died.");
    }

    private void OnEnable()
    {
        _health.Die += Die;
    }

    private void OnDisable()
    {
        _health.Die -= Die;
    }
    
}
