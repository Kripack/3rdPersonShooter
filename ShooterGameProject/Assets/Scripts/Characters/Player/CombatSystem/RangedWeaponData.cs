using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Range Weapon")]
public class RangedWeaponData : WeaponData
{
    [Header("Range Weapon Parameters")]
    public bool useRecoil;
    public float recoilFactor;
    public float bodyRecoilFactor;
    public AnimationClip reloadAnimation;
}
