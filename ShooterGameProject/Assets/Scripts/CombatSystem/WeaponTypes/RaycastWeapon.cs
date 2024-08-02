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
            Debug.Log(hit.transform.name);
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(weaponData.damage);
            }
            
            var impactRotation = Quaternion.LookRotation(hit.normal);
            CreateImpact(rangeData.impactEffect, hit, impactRotation);
        }
    }

    private Vector3 CalculateSpread()
    {
        return new Vector3
        {
            x = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor),
            y = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor),
            z = Random.Range(-rangeData.recoilFactor, rangeData.recoilFactor)
        };
    }
}
