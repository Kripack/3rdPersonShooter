using System;
using UnityEngine;
public class HeadHitbox : Hitbox
{
     public event Action OnHeadShot;
     
     public override void Visit(RaycastWeapon weapon, RaycastHit hit, float damageMultiplier = 1f)
     {
          if (damageable.IsDead) return;
          DefaultVisit(weapon.Data.damage, weapon.Data.headShotMultiplier);
          
          VisualFXManager.instance.SpawnImpactEffect(weapon.Data.impactFXPreset.headShotEffect, hit);
          SoundFXManager.instance.PlayAudioClip(weapon.Data.impactFXPreset.headshotSound, hit.point, 0.75f);
          
          OnHeadShot?.Invoke();
     }
     

     protected override IDamageable GetDamageable()
     {
          return GetComponentInParent<IDamageable>();
     }
}
