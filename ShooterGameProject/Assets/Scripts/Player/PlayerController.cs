using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator Animator;
    public CharacterAnimator CharacterAnimator { get; private set; }
    public bool isPerformingAction;
    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float AirSpeed;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float rotationSpeed;

    public PlayerMovement Motor { get; private set; }
    public InputReader input;
    private ViewController viewController;

    #region States 
    private MovementBaseState currentMovementState;
    public IdleState IdleState { get; private set; }
    public WalkingState WalkingState { get; private set; }
    public RunningState RunningState { get; private set; }
    public CrouchingState CrouchingState { get; private set; }
    public JumpState JumpState { get; private set; }
    public InAirState InAirState { get; private set; }
    #endregion

    private float currentSpeed;

    private void Awake()
    {
        CharacterAnimator = new CharacterAnimator(this);
        Motor = GetComponent<PlayerMovement>();
        viewController = GetComponent<ViewController>();

        IdleState = new IdleState(this);
        WalkingState = new WalkingState(this);
        RunningState = new RunningState(this);
        CrouchingState = new CrouchingState(this);
        JumpState = new JumpState(this);
        InAirState = new InAirState(this);
    }
    void Start()
    {
        SetMovementState(IdleState);

        input.Jump += OnJump;
        input.Roll += OnRoll;
    }

    private void Update()
    {
        CharacterAnimator.UpdateAnimationBlend(input.moveInput);
        currentMovementState.UpdateState();
        //
        if (!Motor.IsGrounded())
        {
            if (currentMovementState != InAirState) { SetMovementState(InAirState); }
        }
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log(currentMovementState.ToString());
        }
    }

    private void FixedUpdate()
    {
        Motor.ApplyGravity();
        if (!isPerformingAction)
        {
            Motor.Rotate(rotationSpeed);
            if (input.moveInput != Vector2.zero)
            {
                Motor.Move(input.moveInput, currentSpeed);
            }
        }
        viewController.CameraControl();
    }
    private void OnJump()
    {
        if (isPerformingAction) return;
        if (Motor.IsGrounded())
        {
            if (currentMovementState == RunningState) CharacterAnimator.PlayTargetActionAnimation("JumpWhileRunning", true);
            else CharacterAnimator.PlayTargetActionAnimation("Jump_Up", true);
        }
    }
    private void OnRoll()
    {
        if (isPerformingAction) return;
        if (input.moveInput == Vector2.zero) return;
        Motor.RotateToMoveDir(input.moveInput);
        CharacterAnimator.PlayTargetActionAnimation("StandToRoll", true, true);
    }

    public void SetMovementState(MovementBaseState newState)
    {
        currentMovementState?.ExitState();
        currentMovementState = newState;
        currentMovementState.EnterState();
    }
    public void SetSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }
    
}
