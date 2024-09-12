using UnityEngine;
using Zenject;

public class AmmoItem : MonoBehaviour
{
    [SerializeField] private int ammoCount;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField][Range(0, 1f)] private float pickupVolume;
    [Inject] private PlayerCombatController _playerCombatController;
    [Inject] private CollectiblesInvoker _invoker;
    private AmmoManager _ammoManager;

    private void Start()
    {
        _ammoManager = _playerCombatController.AmmoManager;
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_ammoManager == null) return;
        
        _ammoManager.AddTotalAmmo(ammoType, ammoCount);
        
        SoundFXManager.instance.PlayAudioClip(pickupClip, transform.position, pickupVolume);
        _invoker.PickUpItem();
        
        Destroy(gameObject);
    }
    
}