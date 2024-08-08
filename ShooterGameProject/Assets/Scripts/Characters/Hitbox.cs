using UnityEngine;

public class Hitbox : MonoBehaviour, IWeaponVisitor
{
    private IDamageable _damageable;

    private void Start()
    {
        _damageable = GetDamageable();
    }

    public virtual void Visit(Weapon weapon, RaycastHit hit)
    {
        DefaultWeaponVisit(weapon);
    }
    
    public virtual void Visit(RaycastWeapon weapon, RaycastHit hit)
    {
        DefaultWeaponVisit(weapon);
        
        VisualFXManager.instance.SpawnImpactEffect(weapon.WeaponData.impactEffectPreset.fleshBodyEffect, hit);
    }
    
    protected void DefaultWeaponVisit(Weapon weapon, float damageMultiplier = 1f)
    {
        var damage = weapon.WeaponData.damage * damageMultiplier;
        _damageable?.TakeDamage(damage);
    }

    protected virtual IDamageable GetDamageable()
    {
        return GetComponent<IDamageable>();
    }
}
