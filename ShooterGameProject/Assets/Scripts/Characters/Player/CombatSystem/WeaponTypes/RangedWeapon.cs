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

    public override void Disable()
    {
        base.Disable();
        characterAnimator.Animator.SetBool(characterAnimator.AimingBool, false);
    }
    
    public override void StartReload()
    {
        if (isReloading) return;
        isReloading = true;

        SoundFXManager.instance.PlaySoundClip(rangeData.reloadSound, weaponAudioSource);
        
        characterAnimator.Animator.SetTrigger(characterAnimator.ReloadTrigger);
        StartCoroutine(WeaponRigDelayOff(rangeData.reloadAnimation.length - 0.5f));
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
        return new Vector3
        {
            x = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor),
            y = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor),
            z = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor)
        };
    }
    
    private IEnumerator WeaponRigDelayOff(float delay)
    {
        characterAnimator.SetAimRigWeight(0f);
        characterAnimator.SetHoldWeaponRigWeight(0f);
        
        yield return new WaitForSeconds(delay);

        characterAnimator.SetAimRigWeight(1f);
        characterAnimator.SetHoldWeaponRigWeight(1f);
    }
    
}
