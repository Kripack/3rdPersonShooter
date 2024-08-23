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
    
    private Weapon _currentWeapon;
    private PlayerController _playerController;
    private Coroutine _autoAttackCoroutine;
    private Coroutine _resetFirstAttackCoroutine;
    private float _nextTimeToFire;

    private void Awake()
    {
        WeaponSelector = new WeaponSelector(this);
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
    }

    private void Attack()
    {
        if (_playerController.IsPerformingAction) return;
        
        if (AmmoManager.UseAmmo(_currentWeapon.Data.ammoType))
        {
            _currentWeapon.Attack();
            OnAttack?.Invoke(_currentWeapon.Data);
        }
        else StartReload();
    }
    private IEnumerator AutoAttackCoroutine()
    {
        while (WeaponEquiped)
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _currentWeapon.Data.fireRate;
                Attack();
                FirstAttackPerformed = true;
            }
            yield return null;
        }
    }
    private void OnStartAutoAttack()
    {
        if (!WeaponEquiped) return;
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
            _currentWeapon.StartReload();
        }
    }
    
    public void ApplyReload()
    {
        if (WeaponEquiped)
        {
            _currentWeapon.ApplyReload();
        }
    }

    public void ApplyBodyRecoil(Vector3 recoilValue)
    {
        weaponHolderFollow.localPosition += recoilValue;
    }
    private void SwitchWeapon(int index)
    {
        if (_currentWeapon != null) 
        { 
            WeaponSelector.DisableWeapon();
            _currentWeapon = null;
        }
        else
        {
            var newWeaponData = WeaponSelector.SelectWeapon(weaponDataArray, index);
            var newWeaponObject = WeaponSelector.ActivateWeapon(newWeaponData, spawnPos, this);
            _currentWeapon = newWeaponObject.GetComponent<Weapon>();
        }
    }
    
    public void SetEquipStatus(bool status)
    {
        WeaponEquiped = status;
    }
}
