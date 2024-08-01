using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon
{
    [SerializeField] protected ParticleSystem muzzleFlash;

    protected RangedWeaponData RangeData;
    public override Weapon Initialize(WeaponData data, CombatSystemController combatSystemController)
    {
        base.Initialize(data, combatSystemController);
        
        CharacterAnimator.Animator.SetBool(CharacterAnimator.AimingBool, true);

        if (data is RangedWeaponData)
        {
            RangeData = (RangedWeaponData) data;
        }
        else { Debug.LogError("This is a Ranged Weapon. The data file should be RangeWeaponData."); }

        return this;
    }

    public override void Disable()
    {
        base.Disable();
        CharacterAnimator.Animator.SetBool(CharacterAnimator.AimingBool, false);
    }
    
    public override void StartReload()
    {
        if (IsReloading) return;
        IsReloading = true;

        SoundFXManager.instance.PlaySoundClip(RangeData.reloadSound, WeaponAudioSource);

        Debug.Log("Reloading...");
        
        CharacterAnimator.Animator.SetTrigger(CharacterAnimator.ReloadTrigger);
        StartCoroutine(WeaponRigDelayOff(RangeData.reloadAnimation.length - 0.5f));
    }
    
    public override void ApplyReload()
    {
        if (!IsReloading) return;
        CombatSystemController.AmmoManager.Reload(RangeData.ammoType);
        IsReloading = false;
        
        Debug.Log("End Reloading.");
    }

    protected override void ReactOnPrimaryAttack()
    {
        base.ReactOnPrimaryAttack();
        
        var recoil = new Vector3(RangeData.bodyRecoilFactor, 0f, 0f);
        CombatSystemController.ApplyBodyRecoil(recoil);
    }
    private IEnumerator WeaponRigDelayOff(float delay)
    {
        CharacterAnimator.SetAimRigWeight(0f);
        CharacterAnimator.SetHoldWeaponRigWeight(0f);
        
        yield return new WaitForSeconds(delay);

        CharacterAnimator.SetAimRigWeight(1f);
        CharacterAnimator.SetHoldWeaponRigWeight(1f);
    }
}
