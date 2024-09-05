using System;
using System.Collections;
using UnityEngine;

public class CombatSystemController : MonoBehaviour
{
    public event Action<WeaponData> OnAttack;
    public InputReader input;
    public WeaponData[] weaponDataArray;
    
    public AmmoManager AmmoManager { get; private set; }
    public CharacterAnimator CharacterAnimator {  get; private set; }
    public Transform spawnPos;
    public bool WeaponEquiped { get; private set; }
    public WeaponSelector WeaponSelector { get; private set; }
    public bool FirstAttackPerformed { get; private set; }
    
    [SerializeField] private Transform weaponHolderFollow;
    
    private Weapon _activeWeapon;
    private PlayerController _playerController;
    private Coroutine _autoAttackCoroutine;
    private Coroutine _resetFirstAttackCoroutine;
    private float _nextTimeToFire;

    private void Awake()
    {
        WeaponSelector = new WeaponSelector(this, spawnPos, weaponDataArray);
        AmmoManager = new AmmoManager();
        CharacterAnimator = GetComponent<CharacterAnimator>();
        _playerController = GetComponent<PlayerController>();

        AmmoManager.Initialize();
    }
    
    private void Start()
    {
        input.SelectWeapon += SwitchWeapon;
        input.Reload += StartReload;
        input.StartAutoAttack += OnStartAutoAttack;
        input.StopAutoAttack += OnStopAutoAttack;
        input.SelectNext += SelectNextWeapon;
        input.SelectPrevious += SelectPreviousWeapon;
    }

    private void OnDisable()
    {
        input.SelectWeapon -= SwitchWeapon;
        input.Reload -= StartReload;
        input.StartAutoAttack -= OnStartAutoAttack;
        input.StopAutoAttack -= OnStopAutoAttack;
        input.SelectNext -= SelectNextWeapon;
        input.SelectPrevious -= SelectPreviousWeapon;
    }

    private void Attack()
    {
        if (_playerController.IsPerformingAction) return;
        
        if (AmmoManager.UseAmmo(_activeWeapon.Data.ammoType))
        {
            _activeWeapon.Attack();
            OnAttack?.Invoke(_activeWeapon.Data);
        }
        else StartReload();
    }
    private IEnumerator AutoAttackCoroutine()
    {
        while (WeaponEquiped)
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _activeWeapon.Data.fireRate;
                Attack();
                FirstAttackPerformed = true;
            }
            yield return null;
        }
    }
    private void OnStartAutoAttack()
    {
        if (!WeaponEquiped || _playerController.IsDead) return;
        if (_autoAttackCoroutine == null)
        {
            _autoAttackCoroutine = StartCoroutine(AutoAttackCoroutine());
        }
    }

    private void OnStopAutoAttack()
    {
        if (_autoAttackCoroutine != null)
        {
            StopCoroutine(_autoAttackCoroutine);
            
            if (_resetFirstAttackCoroutine == null)
            {
                _resetFirstAttackCoroutine = StartCoroutine(ResetFirstAttack());
            }
            else
            {
                StopCoroutine(_resetFirstAttackCoroutine);
                _resetFirstAttackCoroutine = StartCoroutine(ResetFirstAttack());
            }
            
            _autoAttackCoroutine = null;
        }
    }
    
    private IEnumerator ResetFirstAttack()
    {
        yield return new WaitForSeconds(0.6f);
        FirstAttackPerformed = false;
        _resetFirstAttackCoroutine = null;
    }

    private void StartReload()
    {
        if (WeaponEquiped)
        {
            _activeWeapon.StartReload();
        }
    }
    
    public void ApplyReload()
    {
        if (WeaponEquiped)
        {
            _activeWeapon.ApplyReload();
        }
    }

    public void ApplyBodyRecoil(Vector3 recoilValue)
    {
        weaponHolderFollow.localPosition += recoilValue;
    }
    private void SwitchWeapon(int index)
    {
        if (WeaponEquiped) 
        {
            if (WeaponSelector.CurrentWeaponIndex == index)
            {
                WeaponSelector.DisableWeapon(_activeWeapon);
            }
            else
            {
                WeaponSelector.DisableWeapon(_activeWeapon);
                _activeWeapon = WeaponSelector.SelectWeapon(index);
            }
        }
        else
        {
            _activeWeapon = WeaponSelector.SelectWeapon(index);
        }
    }
    
    private void SelectPreviousWeapon()
    {
        if (WeaponEquiped) WeaponSelector.DisableWeapon(_activeWeapon);
        
        if (WeaponSelector.CurrentWeaponIndex == 0)
        {
            WeaponSelector.CurrentWeaponIndex = -1;
            return;
        }

        if (WeaponSelector.CurrentWeaponIndex == -1)
        {
            WeaponSelector.CurrentWeaponIndex = weaponDataArray.Length;
        }
        
        _activeWeapon = WeaponSelector.SelectPreviousWeapon();
    }

    private void SelectNextWeapon()
    {
        if (WeaponEquiped) WeaponSelector.DisableWeapon(_activeWeapon);
        
        if (WeaponSelector.CurrentWeaponIndex == weaponDataArray.Length - 1)
        {
            WeaponSelector.CurrentWeaponIndex = -1;
            return;
        }
        
        _activeWeapon = WeaponSelector.SelectNextWeapon();
    }
    
    public void SetEquipStatus(bool status)
    {
        WeaponEquiped = status;
    }
}
