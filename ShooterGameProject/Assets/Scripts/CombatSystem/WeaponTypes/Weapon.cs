using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandHint;
    
    protected Camera FPSCam;
    protected CharacterAnimator CharacterAnimator;
    protected CombatSystemController CombatSystemController;
    protected AudioSource WeaponAudioSource;

    protected bool IsReloading;
    protected WeaponData WeaponData;
    public virtual Weapon Initialize(WeaponData data, CombatSystemController cSystemController)
    {
        CombatSystemController = cSystemController;
        CharacterAnimator = cSystemController.CharacterAnimator;
        WeaponAudioSource = GetComponent<AudioSource>();
        FPSCam = Camera.main;
        WeaponData = data;
        
        CharacterAnimator.SetLeftHandIKTarget(leftHandIKTarget, leftHandHint);

        CharacterAnimator.SetAimRigWeight(1f);
        CharacterAnimator.SetHoldWeaponRigWeight(1f);
        
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
        CharacterAnimator.SetAimRigWeight(0f);
        CharacterAnimator.SetHoldWeaponRigWeight(0f);
        
        CharacterAnimator.Animator.CrossFade("Empty", 0.1f, 3);
        
        CharacterAnimator.SetLeftHandIKTarget(null, null);
    }
    
    protected void CreateImpact(GameObject impactObject ,Vector3 spawnPosition, Quaternion rotation)
    {
        Instantiate(impactObject, spawnPosition, rotation);
    }
    
    protected virtual void ReactOnPrimaryAttack()
    {
        CameraService.instance.ShakeCamera(WeaponData.cameraShakeDuration, WeaponData.cameraShakeStrength, WeaponData.easeMode);
    }
}
