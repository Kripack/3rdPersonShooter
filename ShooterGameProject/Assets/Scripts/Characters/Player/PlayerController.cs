using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Hitbox))]
public class PlayerController : MonoBehaviour, IDamageable
{
    public InputReader input;
    public GameObject aimIKTarget;
    public Health Health { get; private set; }
    public CharacterAnimator CharacterAnimator { get; private set; }
    public CombatSystemController CombatSystemController { get; private set; }
    public PlayerLocomotion Motor { get; private set; }
    public bool IsPerformingAction { get; set; }
    public bool IsJumped { get; set; }
    public bool IsDead { get; set; }

    #region Parameters
    
    [SerializeField] private float maxHp;
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
    [field: SerializeField] public float CrouchSpeed { get; private set; }
    [field: SerializeField] public float AirSpeed { get; private set; }
    [field: SerializeField] private float RotationSpeed { get; set; }
    
    #endregion
    
    #region States 
    
    private StateMachine _stateMachine;
    public IdleState IdleState { get; private set; }
    public WalkingState WalkingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public CrouchingState CrouchingState { get; private set; }
    public InAirState InAirState { get; private set; }
    
    #endregion

    private CameraService _cameraService;
    private Hitbox _hitbox;
    private Camera _mainCamera;
    private const BodyType BodyType = global::BodyType.FleshBody;
    private float _currentSpeed;
    private bool _lockRotation;
    private bool _isFreeLook;

    private void Awake()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        Motor = GetComponent<PlayerLocomotion>();
        _cameraService = GetComponent<CameraService>();
        CombatSystemController = GetComponent<CombatSystemController>();
        _hitbox = GetComponent<Hitbox>();
        _mainCamera = Camera.main;
        
        _stateMachine = new StateMachine();
        IdleState = new IdleState(_stateMachine,this);
        WalkingState = new WalkingState(_stateMachine,this);
        RunningState = new RunningState(_stateMachine,this);
        CrouchingState = new CrouchingState(_stateMachine,this);
        InAirState = new InAirState(_stateMachine,this);
        this.Health = new Health(maxHp);
    }
    private void Start()
    {
        _stateMachine.SetState(IdleState);
        _hitbox.SetBodyType(BodyType);
        
        input.Jump += OnJump;
        input.Roll += OnRoll;
        input.FreeLook += FreeLook;
        this.Health.Die += Die;
    }

    private void OnDisable()
    {
        input.Jump -= OnJump;
        input.Roll -= OnRoll;
        input.FreeLook -= FreeLook;
        this.Health.Die -= Die;
    }

    private void Die()
    {
        CharacterAnimator.SetHoldWeaponRigWeight(0f);
        IsDead = true;
        CharacterAnimator.Animator.SetTrigger(CharacterAnimator.DeathTrigger);
    }

    private void Update()
    {
        if (IsDead) return;
        
        CharacterAnimator.UpdateAnimationBlend(input.moveInput);
        _stateMachine.Update();
        if (!Motor.IsGrounded())
        {
            if (_stateMachine.CurrentState != InAirState) { _stateMachine.SetState(InAirState); }
        }
    }

    private void FixedUpdate()
    {
        Motor.ApplyGravity();
        if (IsDead) return;
        
        if (!IsPerformingAction)
        {
            if(!_lockRotation) Motor.Rotate(RotationSpeed);
            if (input.moveInput != Vector2.zero)
            {
                Motor.Move(input.moveInput, _currentSpeed);
            }
        }
        _cameraService.CameraControl();
    }
    
    private void OnJump()
    {
        if (IsPerformingAction) return;
        if (!Motor.IsGrounded()) return;
        
        IsJumped = true;
            
        if (CombatSystemController.WeaponEquiped) CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpLegsOnlyAnimation, false);
        else if (_stateMachine.CurrentState is RunningState) CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpWhileRunningAnimation, false);
        else CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpAnimation, false);
    }
    
    private void OnRoll()
    {
        
        //
        
        if (IsPerformingAction || IsJumped || IsDead) return;
        if (_stateMachine.CurrentState is InAirState) return;
        if (input.moveInput == Vector2.zero) return;
        if (CombatSystemController.WeaponEquiped) return;
        
        Motor.RotateToMoveDir(input.moveInput);
        CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.RollAnimation, true, true);
    }

    public void SetSpeed(float newSpeed)
    {
        _currentSpeed = newSpeed;
    }
    
    private void FreeLook()
    {
        _lockRotation = !_lockRotation;
        if(!_isFreeLook) StartCoroutine(FreeLookRoutine());
    }
    
    private IEnumerator FreeLookRoutine()
    {
        aimIKTarget.transform.parent = transform;
        _isFreeLook = true;
        
        yield return new WaitWhile(() => _lockRotation);
        
        aimIKTarget.transform.parent = _mainCamera.transform;
        aimIKTarget.transform.localPosition = new Vector3(0, 0, 20f);
        aimIKTarget.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

        _isFreeLook = false;
    }

    public void TakeDamage(float amount)
    {
        Health.TakeDamage(amount);
    }
    
}
