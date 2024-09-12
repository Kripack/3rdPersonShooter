using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HitEffectScreen : MonoBehaviour
{
    [Inject] private PlayerController _player;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float startAlpha = 1f;
    private Image _hitImage;
    private Coroutine _hitEffectCoroutine;
    
    private void Awake()
    {
        _hitImage = transform.GetComponent<Image>();
    }

    private void OnEnable()
    {
        _player.Health.OnHealthDecrease += OnHit;
    }
    
    private void OnDisable()
    {
        _player.Health.OnHealthDecrease -= OnHit;
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
        _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, startAlpha);
        
        // Затримка перед початком зникнення
        yield return new WaitForSeconds(delayTime);

        // Плавне зменшення альфа-каналу зображення до 0
        var elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(startAlpha - elapsedTime / fadeOutDuration);
            _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, alpha);
            yield return null;
        }

        _hitImage.color = new Color(_hitImage.color.r, _hitImage.color.g, _hitImage.color.b, 0f);
        _hitImage.enabled = false;

        _hitEffectCoroutine = null;
    }
}