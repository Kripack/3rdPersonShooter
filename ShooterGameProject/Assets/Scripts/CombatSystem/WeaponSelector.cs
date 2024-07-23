using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector
{
    public WeaponSelector(CombatSystem combatSystem)
    {
        _combatSystem = combatSystem;
    }
    private Weapon _activeWeapon;
    private CombatSystem _combatSystem;
    public void DisableWeapon()
    {
        _combatSystem.SetEquipStatus(false);

        _activeWeapon.Disable();
        Object.Destroy(_activeWeapon.gameObject);
    }
    public GameObject ActivateWeapon(WeaponData data, Transform spawnPos, CombatSystem combatSystem)
    {
        var newWeaponObject = Object.Instantiate(data.prefab, spawnPos);
        _activeWeapon = newWeaponObject.GetComponent<Weapon>().Initialize(data, combatSystem);
        
        combatSystem.SetEquipStatus(true);
        return newWeaponObject;
    }
    public WeaponData SelectWeapon()
    {
        return null;
    }
}
