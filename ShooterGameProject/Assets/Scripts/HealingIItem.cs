using UnityEngine;
using Zenject;

public class HealingIItem : MonoBehaviour
{
    [SerializeField] private float healAmount;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField][Range(0, 1f)] private float pickupVolume;
    [Inject] private PlayerController _player;
    
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player.Health.CurrentHp >= _player.Health.MaxHp) return;
        
        _player.Health.Heal(healAmount);
        SoundFXManager.instance.PlayAudioClip(pickupClip, transform.position, pickupVolume);
        
        Destroy(gameObject);
    }
}
