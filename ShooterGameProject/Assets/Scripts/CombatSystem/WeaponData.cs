using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Default")]
public class WeaponData : ScriptableObject
{
    [Header("Parameters")]
    public string weaponName;
    public float damage;
    public float fireRate;
        
    public LayerMask layerMask;
    public GameObject prefab;

    [Header("Camera Shake Setting")]
    public float cameraShakeDuration;
    public float cameraShakeStrength;
    public Ease easeMode;

}
