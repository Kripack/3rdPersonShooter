using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LocomotionFXPlayer : MonoBehaviour
{
    [Range(0, 1f)][SerializeField] private float stepsVolume = 0.3f;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] stepsClips;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landingClip;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = transform.GetComponent<AudioSource>();
    }

    public void PlayMovementSound(AnimationEvent animEvent)
    {
        if (animEvent.animatorClipInfo.weight >= 0.5f)
        {
            SoundFXManager.instance.PlayRandomAudioClip(stepsClips, _audioSource, stepsVolume);
        }
    }
    
    public void PlayJumpSound()
    {
        SoundFXManager.instance.PlayAudioClip(jumpClip, _audioSource, stepsVolume);
    }
    
    public void PlayLandingSound()
    {
        SoundFXManager.instance.PlayAudioClip(landingClip, _audioSource, stepsVolume);
    }
    
}
