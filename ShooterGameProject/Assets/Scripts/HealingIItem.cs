using UnityEngine;
using Zenject;

public class HealingIItem : MonoBehaviour
{
    [SerializeField] private float healAmount;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask layerMask;
    [Inject] private PlayerController _player;
    
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_player.Health.CurrentHp >= _player.Health.MaxHp) return;
        
        _player.Health.Heal(healAmount);
        Destroy(gameObject);
    }
}
