using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/ImpactEffectPreset")]
public class ImpactEffectPreset : ScriptableObject
{
    public GameObject environmentImpactEffect;
    public GameObject fleshBodyEffect;
    public GameObject solidBodyEffect;
    public GameObject headShotEffect;
}
