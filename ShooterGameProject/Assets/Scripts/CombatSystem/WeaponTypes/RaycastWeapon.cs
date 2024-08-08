using UnityEngine;

public class RaycastWeapon : RangedWeapon
{
    public override void Attack(float damageMultiplier)
    {
        if (combatSystemController.AmmoManager.UseAmmo(rangeData.ammoType))
        {
            ReactOnPrimaryAttack();
            
            VisualFXManager.instance.PlayParticleSystem(muzzleFlash);
            SoundFXManager.instance.PlaySoundClip(rangeData.shootingSound, weaponAudioSource);
            SoundFXManager.instance.PlayRandomSoundClip(rangeData.bulletShellsSound, weaponAudioSource);

            RaycastShoot();
        }
        else
        {
            SoundFXManager.instance.PlaySoundClip(rangeData.emptyMagazineSound, weaponAudioSource);
            StartReload();
        }
    }

    private void RaycastShoot()
    {
        var direction = rangeData.useRecoil ? fpsCam.transform.forward + CalculateSpread() : fpsCam.transform.forward;
        if (Physics.Raycast(fpsCam.transform.position, direction, out RaycastHit hit, rangeData.range, rangeData.layerMask))
        {
            if (hit.transform.CompareTag("Environment"))
            {
                VisualFXManager.instance.SpawnImpactEffect(WeaponData.impactEffectPreset.environmentImpactEffect, hit);
                
                return;
            }
            
            if (hit.collider.TryGetComponent<IWeaponVisitor>(out var weaponVisitor))
            {
                Accept(weaponVisitor, hit);
            }
        }
    }
    

    private void Accept(IWeaponVisitor weaponVisitor, RaycastHit hit)
    {
        weaponVisitor.Visit(this, hit);
    }
}
