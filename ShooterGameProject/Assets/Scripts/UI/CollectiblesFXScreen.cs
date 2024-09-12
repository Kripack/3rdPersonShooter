using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CollectiblesFXScreen : MonoBehaviour
{
    [Inject] private CollectiblesInvoker _invoker;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float startAlpha = 1f;
    private Image _pickUpImage;
    private Coroutine _pickUpEffectCoroutine;
    
    private void Awake()
    {
        _pickUpImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _invoker.ItemPickedUp += OnItemPickedUp;
    }
    
    private void OnDisable()
    {
        _invoker.ItemPickedUp -= OnItemPickedUp;
    }

    private void OnItemPickedUp()
    {
        if (_pickUpEffectCoroutine != null)
        {
            StopCoroutine(_pickUpEffectCoroutine);
        }

        _pickUpEffectCoroutine = StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        _pickUpImage.enabled = true;
        _pickUpImage.color = new Color(_pickUpImage.color.r, _pickUpImage.color.g, _pickUpImage.color.b, startAlpha);
        
        // Затримка перед початком зникнення
        yield return new WaitForSeconds(delayTime);

        // Плавне зменшення альфа-каналу зображення до 0
        var elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(startAlpha - elapsedTime / fadeOutDuration);
            _pickUpImage.color = new Color(_pickUpImage.color.r, _pickUpImage.color.g, _pickUpImage.color.b, alpha);
            yield return null;
        }

        _pickUpImage.color = new Color(_pickUpImage.color.r, _pickUpImage.color.g, _pickUpImage.color.b, 0f);
        _pickUpImage.enabled = false;

        _pickUpEffectCoroutine = null;
    }
}