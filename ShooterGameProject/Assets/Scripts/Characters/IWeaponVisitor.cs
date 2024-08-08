using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(Weapon weapon, RaycastHit hit);
    public void Visit(RaycastWeapon weapon, RaycastHit hit);
}
