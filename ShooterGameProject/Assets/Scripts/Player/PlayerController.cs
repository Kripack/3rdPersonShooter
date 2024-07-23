using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputReader input;
    public GameObject aimIKTarget;
    public GameObject cinemachineBrain;
    public CharacterAnimator CharacterAnimator { get; private set; }
    public PlayerMovement Motor { get; private set; }

    #region Parameters
    [field: SerializeField] public bool IsPerformingAction { get; set; }
    [field: SerializeField] public bool IsJumped { get; set; }
    [field:SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
    [field: SerializeField] public float CrouchSpeed { get; private set; }
    [field: SerializeField] public float AirSpeed { get; private set; }
    [field: SerializeField] private float RotationSpeed { get; set; }
    #endregion

    #region States 
    private MovementBaseState _currentMovementState;
    public IdleState IdleState { get; private set; }
    public WalkingState WalkingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public CrouchingState CrouchingState { get; private set; }
    public JumpState JumpState { get; private set; }
    public InAirState InAirState { get; private set; }
    #endregion

    private ViewController _viewController;
    public CombatSystem CombatSystem { get; private set; }
    private float _currentSpeed;
    private bool _lockRotation;
    private bool _isFreeLook;
    private void Awake()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        Motor = GetComponent<PlayerMovement>();
        _viewController = GetComponent<ViewController>();
        CombatSystem = GetComponent<CombatSystem>();

        IdleState = new IdleState(this);
        WalkingState = new WalkingState(this);
        RunningState = new RunningState(this);
        CrouchingState = new CrouchingState(this);
        JumpState = new JumpState(this);
        InAirState = new InAirState(this);
    }
    private void Start()
    {
        SetMovementState(IdleState);

        input.Jump += OnJump;
        input.Roll += OnRoll;
        input.FreeLook += FreeLook;
    }
    private void Update()
    {
        CharacterAnimator.UpdateAnimationBlend(input.moveInput);
        _currentMovementState.UpdateState();
        if (!Motor.IsGrounded())
        {
            if (_currentMovementState != InAirState) { SetMovementState(InAirState); }
        }
        // Debug
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log(_currentMovementState.ToString());
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
        _viewController.CameraControl();
    }
    private void OnJump()
    {
        if (IsPerformingAction) return;
        if (!Motor.IsGrounded()) return;
        
        IsJumped = true;
            
        if (CombatSystem.WeaponEquiped) CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpLegsOnlyAnimation, false);
        else if (_currentMovementState is RunningState) CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpWhileRunningAnimation, false);
        else CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.JumpAnimation, false);
    }
    private void OnRoll()
    {
        if (IsPerformingAction || IsJumped) return;
        if (_currentMovementState is InAirState) return;
        if (input.moveInput == Vector2.zero) return;
        if (CombatSystem.WeaponEquiped) CombatSystem.WeaponSelector.DisableWeapon();
        Motor.RotateToMoveDir(input.moveInput);
        CharacterAnimator.PlayTargetActionAnimation(CharacterAnimator.RollAnimation, true, true);
    }
    public void SetMovementState(MovementBaseState newState)
    {
        _currentMovementState?.ExitState();
        _currentMovementState = newState;
        _currentMovementState.EnterState();
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
    
}
