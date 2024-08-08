using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData { get; private set; }
    
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandHint;
    
    protected Camera fpsCam;
    protected CharacterAnimator characterAnimator;
    protected CombatSystemController combatSystemController;
    protected AudioSource weaponAudioSource;
    protected bool isReloading;

    public virtual Weapon Initialize(WeaponData data, CombatSystemController cSystemController)
    {
        combatSystemController = cSystemController;
        characterAnimator = cSystemController.CharacterAnimator;
        weaponAudioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        WeaponData = data;
        
        characterAnimator.SetLeftHandIKTarget(leftHandIKTarget, leftHandHint);

        characterAnimator.SetAimRigWeight(1f);
        characterAnimator.SetHoldWeaponRigWeight(1f);
        
        return this;
    }
    public virtual void Attack(float damageMultiplier)
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
        CameraService.instance.ShakeCamera(WeaponData.cameraShakeDuration, WeaponData.cameraShakeStrength, WeaponData.easeMode);
    }
    
}
