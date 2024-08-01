using UnityEngine;

public class RaycastWeapon : RangedWeapon
{
    public override void Attack(float damageMultiplier)
    {
        if (CombatSystemController.AmmoManager.UseAmmo(RangeData.ammoType))
        {
            ReactOnPrimaryAttack();
            
            VisualFXManager.instance.PlayParticleSystem(muzzleFlash);
            SoundFXManager.instance.PlaySoundClip(RangeData.shootingSound, WeaponAudioSource);
            SoundFXManager.instance.PlayRandomSoundClip(RangeData.bulletShellsSound, WeaponAudioSource);

            RaycastShoot();
        }
        else
        {
            SoundFXManager.instance.PlaySoundClip(RangeData.emptyMagazineSound, WeaponAudioSource);
            StartReload();
        }
    }

    private void RaycastShoot()
    {
        var direction = RangeData.useRecoil ? FPSCam.transform.forward + CalculateSpread() : FPSCam.transform.forward;
        if (Physics.Raycast(FPSCam.transform.position, direction, out RaycastHit hit, RangeData.range, RangeData.layerMask))
        {
            Debug.Log(hit.transform.name);
            var impactRotation = Quaternion.LookRotation(hit.normal);
            CreateImpact(RangeData.impactEffect, hit.point, impactRotation);
        }
    }

    private Vector3 CalculateSpread()
    {
        return new Vector3
        {
            x = Random.Range(-RangeData.recoilFactor, RangeData.recoilFactor),
            y = Random.Range(-RangeData.recoilFactor, RangeData.recoilFactor),
            z = Random.Range(-RangeData.recoilFactor, RangeData.recoilFactor)
        };
    }
}
