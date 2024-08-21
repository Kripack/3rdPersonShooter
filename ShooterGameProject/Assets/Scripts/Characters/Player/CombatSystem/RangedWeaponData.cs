using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Range Weapon")]
public class RangedWeaponData : WeaponData
{
    [Header("Range Weapon Parameters")]
    public bool useRecoil;
    public float recoilFactor;
    public float bodyRecoilFactor;
    
    [Header("Effects")]
    public AudioClip shootingSound;
    public AudioClip reloadSound;
    public AudioClip[] bulletShellsSound;
    
    public AnimationClip reloadAnimation;
}
