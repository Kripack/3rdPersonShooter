using TMPro;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private CombatSystemController combatSystemController;
    private TextMeshProUGUI _text;
    private AmmoType _ammoType;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        combatSystemController.AmmoManager.OnAmmoChanged += UpdateUI;
        combatSystemController.WeaponSelector.OnWeaponSelected += ChangeWeapon;
        combatSystemController.WeaponSelector.OnWeaponDisabled += ClearUI;
    }
    
    private void OnDisable()
    {
        combatSystemController.AmmoManager.OnAmmoChanged -= UpdateUI;
        combatSystemController.WeaponSelector.OnWeaponSelected -= ChangeWeapon;
        combatSystemController.WeaponSelector.OnWeaponDisabled -= ClearUI;
    }

    private void ChangeWeapon(WeaponData newWeapon)
    {
        _ammoType = newWeapon.ammoType;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        _text.text = combatSystemController.AmmoManager.GetCurrentAmmo(_ammoType) + "/" +
                     combatSystemController.AmmoManager.GetMagazineSizes(_ammoType);
    }

    private void ClearUI()
    {
        _text.text = "";
    }
}
