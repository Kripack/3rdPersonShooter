using System;
using UnityEngine;

public class Hitbox : MonoBehaviour, IAttackVisitor
{
    public event Action OnHit;

    protected BodyType _bodyType;
    protected IDamageable damageable;

    public void SetBodyType(BodyType type)
    {
        _bodyType = type;
    }
    
    private void Start()
    {
        damageable = GetDamageable();
    }

    public virtual void Visit(Enemy enemy, RaycastHit hit, float damageMultiplier = 1f)
    {
        if (damageable.IsDead) return;
        DefaultVisit(enemy.Data.damage, damageMultiplier);
        
        var impactRotation = Quaternion.LookRotation(hit.normal);
        PlayImpactFX(enemy.Data.impactFXPreset, hit.point, impactRotation);
    }

    public virtual void Visit(Weapon weapon, RaycastHit hit, float damageMultiplier = 1f)
    {
        DefaultVisit(weapon.Data.damage, damageMultiplier);
    }
    
    public virtual void Visit(RaycastWeapon weapon, RaycastHit hit, float damageMultiplier = 1f)
    {
        if (damageable.IsDead) return;
        DefaultVisit(weapon.Data.damage, damageMultiplier);
        
        var impactRotation = Quaternion.LookRotation(hit.normal);
        PlayImpactFX(weapon.Data.impactFXPreset, hit.point, impactRotation);
    }
    
    protected void DefaultVisit(float damage, float damageMultiplier)
    {
        OnHit?.Invoke();
        
        var totalDamage = damage * damageMultiplier;
        damageable?.TakeDamage(totalDamage);
    }
    
    protected virtual IDamageable GetDamageable()
    {
        return GetComponent<IDamageable>();
    }

    protected virtual void PlayImpactFX(ImpactFXPreset preset, Vector3 position, Quaternion rotation)
    {
        SoundFXManager.instance.PlayRandomAudioClip(preset.hitSound, position);
        
        switch (_bodyType)
        {
            case BodyType.FleshBody :
                VisualFXManager.instance.SpawnImpactEffect(preset.fleshBodyEffect, position, rotation);
                
                break;
            case BodyType.SolidBody :
                VisualFXManager.instance.SpawnImpactEffect(preset.solidBodyEffect, position, rotation);
                break;
        }
    }
}
