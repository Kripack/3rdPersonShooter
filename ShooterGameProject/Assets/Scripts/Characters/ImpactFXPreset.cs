using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Preset/ImpactFXPreset")]
public class ImpactFXPreset : ScriptableObject
{
    public GameObject environmentImpactEffect;
    public GameObject fleshBodyEffect;
    public GameObject solidBodyEffect;
    public GameObject headShotEffect;
    public AudioClip[] hitSound;
    public AudioClip headshotSound;
}
