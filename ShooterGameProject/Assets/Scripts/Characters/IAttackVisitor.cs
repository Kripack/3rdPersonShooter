using UnityEngine;

public interface IAttackVisitor
{
    public void Visit(Enemy enemy, RaycastHit hit, float damageMultiplier);
    public void Visit(Weapon weapon, RaycastHit hit, float damageMultiplier = 1f);
    public void Visit(RaycastWeapon weapon, RaycastHit hit, float damageMultiplier);
}
