using UnityEngine;

public class VisualFXManager : MonoBehaviour
{
    public static VisualFXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayParticleSystem(ParticleSystem particle)
    {
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a particle system that is not set.");
        }
    }
    public void PlayParticleSystem(ParticleSystem particleObject, Vector3 spawnPosition, Quaternion rotation)
    {
        if (particleObject != null)
        {
            Instantiate(particleObject, spawnPosition, rotation);
            particleObject.Play();
            
            Destroy(particleObject.gameObject, particleObject.totalTime);
        }
        else
        {
            Debug.LogWarning("Attempted to play a particle system that is not set.");
        }
    }
}
