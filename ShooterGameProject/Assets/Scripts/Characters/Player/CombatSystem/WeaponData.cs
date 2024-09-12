using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Default")]
public class WeaponData : ScriptableObject
{
    
    public string weaponName;
    public GameObject prefab;
    public Sprite icon;
    [Header("Stats")]
    public float damage;
    public float range;
    public float fireRate;
    public AmmoType ammoType;
    public float crosshairFactor = 1f;
    public float headShotMultiplier = 2f;
    public LayerMask layerMask;
    
    [Header("Effects")]
    public ImpactFXPreset impactFXPreset;
    public WeaponSoundPreset weaponSoundPreset;
    public CameraShakePreset cameraShakePreset;

}
