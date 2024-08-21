using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Hitbox))]
public class PlayerController : MonoBehaviour, IDamageable
{
    public InputReader input;
    public GameObject aimIKTarget;
    public GameObject cinemachineBrain;
    public Health Health { get; private set; }
    public CharacterAnimator CharacterAnimator { get; private set; }
    public CombatSystemController CombatSystemController { get; private set; }
    public PlayerLocomotion Motor { get; private set; }

    #region Parameters
    
    [field: SerializeField] public bool IsPerformingAction { get; set; }
    [field: SerializeField] public bool IsJumped { get; set; }
    [field:SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
    [field: SerializeField] public float CrouchSpeed { get; private set; }
    [field: SerializeField] public float AirSpeed { get; private set; }
    [field: SerializeField] private float RotationSpeed { get; set; }
    [SerializeField] private float maxHp;
    
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
    private BodyType _bodyType = BodyType.FleshBody;
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
        _hitbox.SetBodyType(_bodyType);
        
        input.Jump += OnJump;
        input.Roll += OnRoll;
        input.FreeLook += FreeLook;
        this.Health.Die += Die;
    }

    private void Die()
    {
        return;
    }

    private void Update()
    {
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
        if (IsPerformingAction || IsJumped) return;
        if (_stateMachine.CurrentState is InAirState) return;
        if (input.moveInput == Vector2.zero) return;
        if (CombatSystemController.WeaponEquiped) CombatSystemController.WeaponSelector.DisableWeapon();
        
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
        
        aimIKTarget.transform.parent = cinemachineBrain.transform;
        aimIKTarget.transform.localPosition = new Vector3(0, 0, 20f);
        aimIKTarget.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

        _isFreeLook = false;
    }

    public void TakeDamage(float amount)
    {
        Health.TakeDamage(amount);
    }
}
