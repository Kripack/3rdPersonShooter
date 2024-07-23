using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CombatSystem : MonoBehaviour
{
    public InputReader input;
    public WeaponData weaponData;
    public AmmoManager AmmoManager { get; private set; }
    public CharacterAnimator CharacterAnimator {  get; private set; }
    public Transform spawnPos;
    public bool CanAttack { get; private set; }
    public bool WeaponEquiped { get; private set; }

    public WeaponSelector WeaponSelector { get; private set; }
    private Weapon _activeWeapon;
    private float _damageMultiplier = 1f;
    private PlayerController _playerController;

    private void Awake()
    {
        WeaponSelector = new(this);
        AmmoManager = new();
    }
    private void Start()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        _playerController = GetComponent<PlayerController>();

        AmmoManager.Initialize();

        input.SelectWeapon += SwitchWeapon;
        input.Attack += Attack;

        CanAttack = true;
    }

    private void Attack()
    {
        if (!CanAttack || _playerController.IsPerformingAction) return;
        if (_activeWeapon != null)
        {
            _activeWeapon.Attack(_damageMultiplier);
        }
    }
    public void ApplyReload()
    {
        _activeWeapon.ApplyReload();
    }

    private void SwitchWeapon()
    {
        if (_activeWeapon != null) 
        { 
            WeaponSelector.DisableWeapon();
            _activeWeapon = null;
        }
        else
        {
            //WeaponData newWeaponData = weaponSelector.SelectWeapon();
            GameObject newWeaponObject = WeaponSelector.ActivateWeapon(weaponData, spawnPos, this);
            _activeWeapon = newWeaponObject.GetComponent<Weapon>();
        }
    }
    public void ResetAttack()
    {
        CanAttack = true;
    }
    public void BanAttack()
    {
        CanAttack = false;
    }

    public void SetEquipStatus(bool status)
    {
        WeaponEquiped = status;
    }
}
