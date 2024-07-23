using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Serialization;

public class CharacterAnimator : MonoBehaviour
{
    public Animator Animator { get; private set; }
    [field: SerializeField] public Rig AimRig { get; private set; }
    [field: SerializeField] public Rig HoldWeaponRig { get; private set; }
    [field: SerializeField] public float SpineAimWeight { get; private set; }
    [field: SerializeField] public float SpineAimSprintWeight { get; private set; }
    public RigBuilder RigBuilder{ get; private set; }
    
    [SerializeField] public MultiAimConstraint spineAimConstrain;
    [SerializeField] private TransformFollow leftHandTarget;
    [SerializeField] private TransformFollow leftHandHint;
    
    private PlayerController _controller;

    #region Animation HashID
    
    public int JumpAnimation { get; private set; }
    public int JumpWhileRunningAnimation { get; private set; }
    public int JumpLegsOnlyAnimation { get; private set; }
    public int FallingLoop { get; private set; }
    public int FallingLoopLegsOnly { get; private set; }
    public int LandingAnimation { get; private set; }
    public int LandingLegsOnlyAnimation { get; private set; }
    public int RollAnimation { get; private set; }
    public int WalkingBool { get; private set; }
    public int CrouchingBool { get; private set; }
    public int RunningBool { get; private set; }
    public int AimingBool { get; private set; }
    public int LandingTrigger { get; private set; }
    public int ReloadTrigger { get; private set; }
    private int _vertical;
    private int _horizontal;

    #endregion

    public void Awake()
    {
        SetAnimatorHashIDs();
    }

    public void Start()
    {
        _controller = GetComponent<PlayerController>();
        Animator = GetComponent<Animator>();
        RigBuilder = GetComponent<RigBuilder>();

        SpineAimWeight = spineAimConstrain.weight;
    }
    public void UpdateAnimationBlend(Vector2 moveInput)
    {
       Animator.SetFloat(_vertical, moveInput.y, 0.1f, Time.deltaTime);
       Animator.SetFloat(_horizontal, moveInput.x, 0.1f, Time.deltaTime);
    }
    public void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = false, float normalizedTransitionDuration = 0.2f)
    {
        if (_controller.IsPerformingAction) return;

        Animator.applyRootMotion = applyRootMotion;
        _controller.IsPerformingAction = isPerformingAction;

        Animator.CrossFade(targetAnimation, normalizedTransitionDuration);
    }
    public void PlayTargetActionAnimation(int hashID, bool isPerformingAction, bool applyRootMotion = false, float normalizedTransitionDuration = 0.2f)
    {
        if (_controller.IsPerformingAction) return;

        Animator.applyRootMotion = applyRootMotion;
        _controller.IsPerformingAction = isPerformingAction;

        Animator.CrossFade(hashID, normalizedTransitionDuration);
    }
    public void SetLeftHandIKTarget(Transform leftTarget, Transform leftHint)
    {
        leftHandTarget.SetTarget(leftTarget);
        leftHandHint.SetTarget(leftHint);
    }
    public void SetSpineAimWeight(float weight)
    {
        spineAimConstrain.weight = weight;
    }
    public void SetAimRigWeight(float weight)
    {
        AimRig.weight = weight;
    }
    public void SetHoldWeaponRigWeight(float weight)
    {
        HoldWeaponRig.weight = weight;
    }
    private void SetAnimatorHashIDs()
    {
        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
        WalkingBool = Animator.StringToHash("Walking");
        CrouchingBool = Animator.StringToHash("Crouching");
        RunningBool = Animator.StringToHash("Running");
        AimingBool = Animator.StringToHash("GunAiming");
        LandingTrigger = Animator.StringToHash("Landing");
        ReloadTrigger = Animator.StringToHash("Reload");
        
        JumpAnimation = Animator.StringToHash("Jump_Up");
        JumpLegsOnlyAnimation = Animator.StringToHash("Jump_Up_LegsOnly");
        JumpWhileRunningAnimation = Animator.StringToHash("JumpWhileRunning");
        RollAnimation = Animator.StringToHash("StandToRoll");
        FallingLoop = Animator.StringToHash("FallingLoop");
        FallingLoopLegsOnly = Animator.StringToHash("FallingLoop_LegsOnly");
        LandingAnimation = Animator.StringToHash("Landing");
        LandingLegsOnlyAnimation = Animator.StringToHash("Landing_LegsOnly");

    }
    
}
