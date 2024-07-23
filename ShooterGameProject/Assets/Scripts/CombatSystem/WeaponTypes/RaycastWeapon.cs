using System.Collections;
using UnityEngine;

public class RaycastWeapon : RangedWeapon
{
    public override void Attack(float damageMultiplier)
    {
        if (CombatSystem.AmmoManager.UseAmmo(RangeData.ammoType))
        {
            VisualFXManager.instance.PlayParticleSystem(muzzleFlash);
            SoundFXManager.instance.PlaySoundClip(RangeData.shootingSound, WeaponAudioSource);

            if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out RaycastHit hit, RangeData.range, RangeData.layerMask))
            {
                Debug.Log(hit.transform.name);
                var impactRotation = Quaternion.LookRotation(hit.normal);
                CreateImpact(RangeData.impactEffect, hit.point, impactRotation);
            }
        }
        else
        {
            SoundFXManager.instance.PlaySoundClip(RangeData.emptyMagazineSound, WeaponAudioSource);
            StartReload();
        }
    }
    private void StartReload()
    {
        if (IsReloading) return;
        IsReloading = true;

        SoundFXManager.instance.PlaySoundClip(RangeData.reloadSound, WeaponAudioSource);

        Debug.Log("Reloading...");
        
        CharacterAnimator.Animator.SetTrigger(CharacterAnimator.ReloadTrigger);
        StartCoroutine(WeaponRigDelayOff(RangeData.reloadAnimation.length - 0.5f));
    }
    private IEnumerator WeaponRigDelayOff(float delay)
    {
        CharacterAnimator.SetAimRigWeight(0f);
        CharacterAnimator.SetHoldWeaponRigWeight(0f);
        
        yield return new WaitForSeconds(delay);

        CharacterAnimator.SetAimRigWeight(1f);
        CharacterAnimator.SetHoldWeaponRigWeight(1f);
    }
    public override void ApplyReload()
    {
        CombatSystem.AmmoManager.Reload(RangeData.ammoType);
        IsReloading = false;
        
        Debug.Log("End Reloading.");
    }


}
