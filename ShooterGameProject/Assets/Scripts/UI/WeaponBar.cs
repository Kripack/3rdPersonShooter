using System;
using TMPro;
using UnityEngine;

public class WeaponBar : MonoBehaviour
{
    [SerializeField] private CombatSystemController combatSystemController;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        combatSystemController.WeaponSelector.OnWeaponSelected += UpdateUI;
        combatSystemController.WeaponSelector.OnWeaponDisabled += ClearUI;
    }
    
    private void UpdateUI(WeaponData data)
    {
        _text.text = data.weaponName;
    }

    private void ClearUI()
    {
        _text.text = "";
    }
}
