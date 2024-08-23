using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBar : MonoBehaviour
{
    [SerializeField] private CombatSystemController combatSystemController;
    private TextMeshProUGUI _text;
    private Image _icon;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _icon = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        combatSystemController.WeaponSelector.OnWeaponSelected += UpdateUI;
        combatSystemController.WeaponSelector.OnWeaponDisabled += ClearUI;
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
