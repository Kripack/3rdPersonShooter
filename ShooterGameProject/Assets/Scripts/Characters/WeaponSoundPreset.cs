using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Preset/WeaponSoundPreset")]
public class WeaponSoundPreset : ScriptableObject
{
    public AudioClip shootingSound;
    public AudioClip reloadSound;
    public AudioClip[] bulletShellsSound;
}