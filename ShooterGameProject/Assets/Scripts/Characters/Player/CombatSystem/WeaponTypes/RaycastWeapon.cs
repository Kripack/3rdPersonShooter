using UnityEngine;

public class RaycastWeapon : RangedWeapon
{
    public override void Attack()
    {
        base.Attack();
        RaycastShoot();
    }

    private void RaycastShoot()
    {
        var direction = rangeData.useRecoil ? fpsCam.transform.forward + CalculateSpread() : fpsCam.transform.forward;
        if (Physics.Raycast(fpsCam.transform.position, direction, out var hit, Data.range, Data.layerMask))
        {
            if (hit.transform.CompareTag("Environment"))
            {
                VisualFXManager.instance.SpawnImpactEffect(Data.impactEffectPreset.environmentImpactEffect, hit);
                
                return;
            }
            
            if (hit.collider.TryGetComponent<IAttackVisitor>(out var attackVisitor))
            {
                AcceptAttack(attackVisitor, hit);
            }
        }
    }

    private void AcceptAttack(IAttackVisitor attackVisitor, RaycastHit hit)
    {
        attackVisitor.Visit(this, hit);
    }
}
