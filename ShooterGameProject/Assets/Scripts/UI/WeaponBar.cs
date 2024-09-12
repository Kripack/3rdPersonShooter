using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    [FormerlySerializedAs("combatSystemController")] [SerializeField] private PlayerCombatController playerCombatController;
    private TextMeshProUGUI _text;
    private Image _icon;

    private void Awake()
    {
        _text = transform.GetComponent<TextMeshProUGUI>();
        _icon = transform.GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        playerCombatController.WeaponSelector.OnWeaponSelected += UpdateUI;
        playerCombatController.WeaponSelector.OnWeaponDisabled += ClearUI;
    }
    
    private void OnDisable()
    {
        playerCombatController.WeaponSelector.OnWeaponSelected -= UpdateUI;
        playerCombatController.WeaponSelector.OnWeaponDisabled -= ClearUI;
    }
    
    private void UpdateUI(WeaponData data)
    {
        _text.text = data.weaponName;
        
        _icon.color = Color.white;
        _icon.sprite = data.icon;
    }

    private void ClearUI()
    {
        _text.text = "";
        
        _icon.color = Color.clear;
        _icon.sprite = null;
    }
}
