using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealEffectScreen : MonoBehaviour
{
    [Inject] private PlayerController _player;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    
    private Image _healImage;
    private Coroutine _healEffectCoroutine;
    
    private void Awake()
    {
        _healImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _player.Health.OnHealthIncrease += OnHeal;
    }
    
    private void OnDisable()
    {
        _player.Health.OnHealthIncrease -= OnHeal;
    }

    private void OnHeal()
    {
        if (_healEffectCoroutine != null)
        {
            StopCoroutine(_healEffectCoroutine);
        }

        _healEffectCoroutine = StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        _healImage.enabled = true;
        _healImage.color = new Color(_healImage.color.r, _healImage.color.g, _healImage.color.b, 0.5f);
        
        // Затримка перед початком зникнення
        yield return new WaitForSeconds(delayTime);

        // Плавне зменшення альфа-каналу зображення до 0
        var elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(0.5f - elapsedTime / fadeOutDuration);
            _healImage.color = new Color(_healImage.color.r, _healImage.color.g, _healImage.color.b, alpha);
            yield return null;
        }

        _healImage.color = new Color(_healImage.color.r, _healImage.color.g, _healImage.color.b, 0f);
        _healImage.enabled = false;

        _healEffectCoroutine = null;
    }
}