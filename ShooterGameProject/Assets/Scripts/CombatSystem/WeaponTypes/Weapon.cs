using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandHint;
    
    protected Camera fpsCam;
    protected CharacterAnimator characterAnimator;
    protected CombatSystemController combatSystemController;
    protected AudioSource weaponAudioSource;

    protected bool isReloading;
    protected WeaponData weaponData;
    public virtual Weapon Initialize(WeaponData data, CombatSystemController cSystemController)
    {
        combatSystemController = cSystemController;
        characterAnimator = cSystemController.CharacterAnimator;
        weaponAudioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        weaponData = data;
        
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
    
    protected void CreateImpact(GameObject impactObject ,Vector3 spawnPosition, Quaternion rotation)
    {
        var impact = Instantiate(impactObject, spawnPosition, rotation);
    }
    protected void CreateImpact(GameObject impactObject ,RaycastHit hit, Quaternion rotation)
    {
        var impact = Instantiate(impactObject, hit.point, rotation);
        impact.transform.parent = hit.transform;
    }
    
    protected virtual void ReactOnPrimaryAttack()
    {
        CameraService.instance.ShakeCamera(weaponData.cameraShakeDuration, weaponData.cameraShakeStrength, weaponData.easeMode);
    }
}
