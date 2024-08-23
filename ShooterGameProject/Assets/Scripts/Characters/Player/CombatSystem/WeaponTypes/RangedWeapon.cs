using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
    [SerializeField] protected ParticleSystem muzzleFlash;

    protected RangedWeaponData rangeData;
    public override Weapon Initialize(WeaponData data, CombatSystemController cSystemController)
    {
        base.Initialize(data, cSystemController);
        
        characterAnimator.Animator.SetBool(characterAnimator.AimingBool, true);

        if (data is RangedWeaponData)
        {
            rangeData = (RangedWeaponData) data;
        }
        else { Debug.LogError("This is a Ranged Weapon. The data file should be RangeWeaponData."); } // як веде себе в білді?

        return this;
    }

    public override void Attack()
    {
        ReactOnPrimaryAttack();
        
        VisualFXManager.instance.PlayParticleSystem(muzzleFlash); 
        SoundFXManager.instance.PlayAudioClip(rangeData.shootingSound, weaponAudioSource); 
        SoundFXManager.instance.PlayRandomAudioClip(rangeData.bulletShellsSound, weaponAudioSource);
    }

    public override void Disable()
    {
        base.Disable();
        characterAnimator.Animator.SetBool(characterAnimator.AimingBool, false);
    }
    
    public override void StartReload()
    {
        if (isReloading) return;
        isReloading = true;

        SoundFXManager.instance.PlayAudioClip(rangeData.reloadSound, weaponAudioSource);
        
        characterAnimator.Animator.SetTrigger(characterAnimator.ReloadTrigger);
        StartCoroutine(WeaponRigDelay(rangeData.reloadAnimation.length - 0.5f));
    }
    
    public override void ApplyReload()
    {
        if (!isReloading) return;
        combatSystemController.AmmoManager.Reload(rangeData.ammoType);
        isReloading = false;
    }

    protected override void ReactOnPrimaryAttack()
    {
        base.ReactOnPrimaryAttack();
        
        var recoil = new Vector3(rangeData.bodyRecoilFactor, 0f, 0f);
        combatSystemController.ApplyBodyRecoil(recoil);
    }
    
    protected Vector3 CalculateSpread()
    {
        var recoilFactor = rangeData.recoilFactor;
        if (!combatSystemController.FirstAttackPerformed) recoilFactor = rangeData.recoilFactor / 3f;
        
        return new Vector3
        {
            x = Random.Range(-recoilFactor, recoilFactor),
            y = Random.Range(-recoilFactor, recoilFactor),
            z = Random.Range(-recoilFactor, recoilFactor)
        };
    }
    
    private IEnumerator WeaponRigDelay(float delay)
    {
        characterAnimator.SetAimRigWeight(0f);
        characterAnimator.SetHoldWeaponRigWeight(0f);
        
        yield return new WaitForSeconds(delay);

        characterAnimator.SetAimRigWeight(1f);
        characterAnimator.SetHoldWeaponRigWeight(1f);
    }
    
}
