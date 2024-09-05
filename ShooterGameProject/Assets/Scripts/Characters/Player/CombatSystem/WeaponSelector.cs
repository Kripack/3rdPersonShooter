using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class WeaponSelector
{
    public event Action<WeaponData> OnWeaponSelected;
    public event Action OnWeaponDisabled; 
    public int CurrentWeaponIndex { get; set; }

    private WeaponData[] _weaponDataArray;
    private readonly CombatSystemController _combatSystemController;
    private readonly Transform _spawnPos;
    
    public WeaponSelector(CombatSystemController combatSystemController, Transform spawnPos, WeaponData[] weaponDataArray)
    {
        _combatSystemController = combatSystemController;
        _spawnPos = spawnPos;
        _weaponDataArray = weaponDataArray;
        CurrentWeaponIndex = -1;
    }
    
    public void DisableWeapon(Weapon currentWeapon)
    {
        _combatSystemController.SetEquipStatus(false);
        OnWeaponDisabled?.Invoke();
        
        currentWeapon.Disable();
        Object.Destroy(currentWeapon.gameObject);
    }
    
    private Weapon ActivateWeapon(WeaponData data)
    {
        var newWeaponObject = Object.Instantiate(data.prefab, _spawnPos);
        var newWeapon = newWeaponObject.GetComponent<Weapon>().Initialize(data, _combatSystemController);
        
        _combatSystemController.SetEquipStatus(true);
        return newWeapon;
    }
    
    public Weapon SelectWeapon(int index)
    {
        CurrentWeaponIndex = index % _weaponDataArray.Length;
        var selectedWeapon = _weaponDataArray[CurrentWeaponIndex];
        
        OnWeaponSelected?.Invoke(selectedWeapon);
        return ActivateWeapon(selectedWeapon);
    }
    
    public Weapon SelectNextWeapon()
    {
        var newWeaponIndex = CurrentWeaponIndex + 1;
        return SelectWeapon(newWeaponIndex);
    }
    
    public Weapon SelectPreviousWeapon()
    {
        var newWeaponIndex = CurrentWeaponIndex - 1;
        return SelectWeapon(newWeaponIndex);
    }
}
