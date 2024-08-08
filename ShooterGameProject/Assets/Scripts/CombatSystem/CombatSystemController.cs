using System.Collections;
using UnityEngine;

public class CombatSystemController : MonoBehaviour
{
    public InputReader input;
    public WeaponData[] weaponDataArray;
    public AmmoManager AmmoManager { get; private set; }
    public CharacterAnimator CharacterAnimator {  get; private set; }
    public Transform spawnPos;
    public bool WeaponEquiped { get; private set; }
    public WeaponSelector WeaponSelector { get; private set; }
    [SerializeField] private Transform weaponHolderFollow;
    private Weapon _activeWeapon;
    private WeaponData _activeWeaponData;
    private float _damageMultiplier = 1f;
    private PlayerController _playerController;
    private Coroutine _autoAttackCoroutine;
    private float _nextTimeToFire;

    private void Awake()
    {
        WeaponSelector = new WeaponSelector(this);
        AmmoManager = new AmmoManager();
    }
    
    private void Start()
    {
        CharacterAnimator = GetComponent<CharacterAnimator>();
        _playerController = GetComponent<PlayerController>();

        AmmoManager.Initialize();

        input.SelectWeapon += SwitchWeapon;
        input.Reload += StartReload;
        input.StartAutoAttack += OnStartAutoAttack;
        input.StopAutoAttack += OnStopAutoAttack;

    }

    private void Attack()
    {
        if (_playerController.IsPerformingAction) return;
        if (WeaponEquiped)
        {
            _activeWeapon.Attack(_damageMultiplier);
        }
    }
    private IEnumerator AutoAttackCoroutine()
    {
        while (true)
        {
            if (Time.time >= _nextTimeToFire)
            {
                _nextTimeToFire = Time.time + 1f / _activeWeaponData.fireRate;
                Attack();
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
            _autoAttackCoroutine = null;
        }
    }
    private void StartReload()
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.StartReload();
        }
    }
    
    public void ApplyReload()
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.ApplyReload();
        }
    }

    public void ApplyBodyRecoil(Vector3 recoilValue)
    {
        weaponHolderFollow.localPosition += recoilValue;
    }
    private void SwitchWeapon()
    {
        if (_activeWeapon != null) 
        { 
            WeaponSelector.DisableWeapon();
            _activeWeapon = null;
            _activeWeaponData = null;
        }
        else
        {
            WeaponData newWeaponData = WeaponSelector.SelectNextWeapon(weaponDataArray);
            GameObject newWeaponObject = WeaponSelector.ActivateWeapon(newWeaponData, spawnPos, this);
            _activeWeapon = newWeaponObject.GetComponent<Weapon>();
            _activeWeaponData = newWeaponData;
        }
    }
    
    public void SetEquipStatus(bool status)
    {
        WeaponEquiped = status;
    }
}
