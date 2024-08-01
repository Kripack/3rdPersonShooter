using System;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData data;
    private Health _health;
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyBaseState _wanderState;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _health = new Health(data.maxHp);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = data.speed;
        _animator = GetComponentInChildren<Animator>();
        
        _wanderState = new EnemyWanderState(_stateMachine, _animator, _agent, this, 10f);
        _stateMachine.SetState(_wanderState);
    }

    private void Update()
    {
        //_stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }
    private void Die()
    {
        return;
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
