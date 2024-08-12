using UnityEngine;

public interface IAttackVisitor
{
    public void Visit(Enemy enemy, RaycastHit hit);
    public void Visit(Weapon weapon, RaycastHit hit);
    public void Visit(RaycastWeapon weapon, RaycastHit hit);
}
