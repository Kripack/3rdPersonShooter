using TMPro;
using UnityEngine;
using Zenject;

public class AmmoBar : MonoBehaviour
{
    [Inject] private PlayerCombatController _playerCombatController;
    [SerializeField] private TextMeshProUGUI magazineText;
    [SerializeField] private TextMeshProUGUI totalText;
    private AmmoType _ammoType;

    private void OnEnable()
    {
        _playerCombatController.AmmoManager.OnAmmoChanged += UpdateUI;
        _playerCombatController.WeaponSelector.OnWeaponSelected += ChangeWeapon;
        _playerCombatController.WeaponSelector.OnWeaponDisabled += ClearUI;
    }
    
    private void OnDisable()
    {
        _playerCombatController.AmmoManager.OnAmmoChanged -= UpdateUI;
        _playerCombatController.WeaponSelector.OnWeaponSelected -= ChangeWeapon;
        _playerCombatController.WeaponSelector.OnWeaponDisabled -= ClearUI;
    }

    private void ChangeWeapon(WeaponData newWeapon)
    {
        _ammoType = newWeapon.ammoType;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (!_playerCombatController.WeaponEquiped) return;
        
        magazineText.text = _playerCombatController.AmmoManager.GetCurrentAmmo(_ammoType) + "/" +
                     _playerCombatController.AmmoManager.GetMagazineSizes(_ammoType);
        totalText.text = _playerCombatController.AmmoManager.GetTotalAmmo(_ammoType).ToString();
    }

    private void ClearUI()
    {
        magazineText.text = "";
        totalText.text = "";
    }
}
