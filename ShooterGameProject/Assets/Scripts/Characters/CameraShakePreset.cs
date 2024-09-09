using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Preset/CameraShakePreset")]
public class CameraShakePreset : ScriptableObject
{
    [Header("Camera Shake Setting")]
    public float duration = 0.1f; 
    public float strength = 0.05f;
    public Ease easeMode = Ease.InOutBounce;
}