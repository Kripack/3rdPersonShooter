using System;
using UnityEngine;
public class HeadHitbox : Hitbox
{
     public event Action OnHeadShot;
     
     public override void Visit(RaycastWeapon weapon, RaycastHit hit)
     {
          if (damageable.IsDead) return;
          DefaultVisit(weapon.Data.damage, weapon.Data.headShotMultiplier);
          VisualFXManager.instance.SpawnImpactEffect(weapon.Data.impactEffectPreset.headShotEffect, hit);
          
          OnHeadShot?.Invoke();
     }
     
     protected override IDamageable GetDamageable()
     {
          return GetComponentInParent<IDamageable>();
     }
}
