using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectScreen : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    private Image _hitImage;
    private Coroutine _hitEffectCoroutine;
    
    private void Awake()
    {
        _hitImage = transform.GetComponent<Image>();
    }

    private void Start()
    {
        player.Health.OnHealthChange += OnHit;
    }

    private void OnHit()
    {
        if (_hitEffectCoroutine != null)
        {
            StopCoroutine(_hitEffectCoroutine);
        }

        _hitEffectCoroutine = StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        _hitImage.enabled = true;
        _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, 1f);
        
        // Затримка перед початком зникнення
        yield return new WaitForSeconds(delayTime);

        // Плавне зменшення альфа-каналу зображення до 0
        var elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - elapsedTime / fadeOutDuration);
            _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, alpha);
            yield return null;
        }

        _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, 0f);
        _hitImage.enabled = false;

        _hitEffectCoroutine = null;
    }
}