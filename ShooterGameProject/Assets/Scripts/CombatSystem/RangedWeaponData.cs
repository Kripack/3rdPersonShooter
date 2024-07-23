using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Range Weapon")]
public class RangedWeaponData : WeaponData
{
    [Header("Range Weapon Parameters")]
    public AmmoType ammoType;
    public float range;
    public float reloadTime;
    
    [Header("Sounds")]
    public AudioClip shootingSound;
    public AudioClip emptyMagazineSound;
    public AudioClip reloadSound;

    public GameObject impactEffect;
    public AnimationClip reloadAnimation;
}
