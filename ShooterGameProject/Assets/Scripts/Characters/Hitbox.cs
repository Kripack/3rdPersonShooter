using System;
using UnityEngine;

public class Hitbox : MonoBehaviour, IAttackVisitor
{
    public event Action OnHit;

    private BodyType _bodyType;
    private IDamageable _damageable;

    public void SetBodyType(BodyType type)
    {
        _bodyType = type;
    }
    
    private void Start()
    {
        _damageable = GetDamageable();
    }

    public virtual void Visit(Enemy enemy, RaycastHit hit)
    {
        DefaultVisit(enemy.Data.damage);
        
        var impactRotation = Quaternion.LookRotation(hit.normal);
        PlayImpactFX(enemy.Data.impactEffectPreset, hit.point, impactRotation, _bodyType);
    }

    public virtual void Visit(Weapon weapon, RaycastHit hit)
    {
        DefaultVisit(weapon.Data.damage);
    }
    
    public virtual void Visit(RaycastWeapon weapon, RaycastHit hit)
    {
        DefaultVisit(weapon.Data.damage);
        
        var impactRotation = Quaternion.LookRotation(hit.normal);
        PlayImpactFX(weapon.Data.impactEffectPreset, hit.point, impactRotation, _bodyType);
    }
    
    protected void DefaultVisit(float damage, float damageMultiplier = 1f)
    {
        OnHit?.Invoke();
        
        var totalDamage = damage * damageMultiplier;
        _damageable?.TakeDamage(totalDamage);
    }
    
    protected virtual IDamageable GetDamageable()
    {
        return GetComponent<IDamageable>();
    }

    protected virtual void PlayImpactFX(ImpactEffectPreset preset, Vector3 position, Quaternion rotation, BodyType bodyType)
    {
        switch (bodyType)
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
