using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayAudioClip(AudioClip clip, Vector3 spawnPosition, float volume = 1f)
    {
        if (clip != null)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnPosition, Quaternion.identity);
        
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
        else
        {
            Debug.LogWarning("Attempted to play a sound clip that is not set.");
        }
    }
    
    public void PlayAudioClip(AudioClip clip, AudioSource audioSource, float volume = 1f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("Attempted to play a sound clip that is not set.");
        }
    }
    
    public void PlayRandomAudioClip(AudioClip[] clips, AudioSource audioSource, float volume = 1f)
    {
        if (clips != null)
        {
            int randomIndex = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[randomIndex], volume);
        }
        else
        {
            Debug.LogWarning("Attempted to play a sound clip that is not set.");
        }
    }
}
