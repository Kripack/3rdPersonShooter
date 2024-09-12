using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public WeaponData Data { get; protected set; }
    
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandHint;
    
    protected Camera fpsCam;
    protected CharacterAnimator characterAnimator;
    protected PlayerCombatController playerCombatController;
    protected AudioSource weaponAudioSource;
    protected bool isReloading;

    public virtual Weapon Initialize(WeaponData data, PlayerCombatController cController)
    {
        playerCombatController = cController;
        characterAnimator = cController.CharacterAnimator;
        weaponAudioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        Data = data;
        
        characterAnimator.SetLeftHandIKTarget(leftHandIKTarget, leftHandHint);

        characterAnimator.SetAimRigWeight(1f);
        characterAnimator.SetHoldWeaponRigWeight(1f);
        
        return this;
    }
    public virtual void Attack()
    {
        return;
    }
    public virtual void ApplyReload()
    {
        return;
    }

    public virtual void StartReload()
    {
        return;
    }
    
    public virtual void Disable()
    {
        characterAnimator.SetAimRigWeight(0f);
        characterAnimator.SetHoldWeaponRigWeight(0f);
        
        characterAnimator.Animator.CrossFade("Empty", 0.1f, 3);
        
        characterAnimator.SetLeftHandIKTarget(null, null);
    }
    
    protected virtual void ReactOnPrimaryAttack()
    {
        CameraService.instance.ShakeCamera(Data.cameraShakePreset);
    }
    
}
