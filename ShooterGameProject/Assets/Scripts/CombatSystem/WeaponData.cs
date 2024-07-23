using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData/Default")]
public class WeaponData : ScriptableObject
{
    [FormerlySerializedAs("WeaponName")] [Header("Parameters")]
    public string weaponName;
    public float damage;

    public LayerMask layerMask;

    public GameObject prefab;
}
