using System;
using UnityEngine;
public class HeadHitbox : Hitbox
{
     public event Action OnHeadShot;
     
     public override void Visit(RaycastWeapon weapon, RaycastHit hit)
     {
          DefaultWeaponVisit(weapon, weapon.WeaponData.headShotMultiplier);
          VisualFXManager.instance.SpawnImpactEffect(weapon.WeaponData.impactEffectPreset.headShotEffect, hit);
          
          OnHeadShot?.Invoke();
     }
     
     protected override IDamageable GetDamageable()
     {
          return GetComponentInParent<IDamageable>();
     }
}
