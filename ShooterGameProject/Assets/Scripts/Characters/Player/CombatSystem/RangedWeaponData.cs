using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Range Weapon")]
public class RangedWeaponData : WeaponData
{
    [Header("Range Weapon Parameters")]
    public AmmoType ammoType;
    public float range;
    public float reloadTime;
    public bool useRecoil;
    public float recoilFactor;
    public float bodyRecoilFactor;
    
    [Header("Effects")]
    public AudioClip shootingSound;
    public AudioClip emptyMagazineSound;
    public AudioClip reloadSound;
    public AudioClip[] bulletShellsSound;
    
    public AnimationClip reloadAnimation;
}
