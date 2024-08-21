using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class WeaponSelector
{
    public event Action<WeaponData> OnWeaponSelected;
    public event Action OnWeaponDisabled; 
    private int _currentWeaponIndex = -1;
    
    public WeaponSelector(CombatSystemController combatSystemController)
    {
        _combatSystemController = combatSystemController;
    }
    
    private Weapon _activeWeapon;
    private CombatSystemController _combatSystemController;
    
    public void DisableWeapon()
    {
        _combatSystemController.SetEquipStatus(false);
        OnWeaponDisabled?.Invoke();
        
        _activeWeapon.Disable();
        Object.Destroy(_activeWeapon.gameObject);
    }
    
    public GameObject ActivateWeapon(WeaponData data, Transform spawnPos, CombatSystemController combatSystemController)
    {
        var newWeaponObject = Object.Instantiate(data.prefab, spawnPos);
        _activeWeapon = newWeaponObject.GetComponent<Weapon>().Initialize(data, combatSystemController);
        
        combatSystemController.SetEquipStatus(true);
        return newWeaponObject;
    }
    
    public WeaponData SelectNextWeapon(WeaponData[] weaponDataArray)
    {
        _currentWeaponIndex = (_currentWeaponIndex + 1) % weaponDataArray.Length;
        WeaponData selectedWeapon = weaponDataArray[_currentWeaponIndex];
        OnWeaponSelected?.Invoke(selectedWeapon);
        return selectedWeapon;
    }
    
}
