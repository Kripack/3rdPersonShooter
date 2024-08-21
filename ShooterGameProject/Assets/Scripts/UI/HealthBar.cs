using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _text.text = player.Health.CurrentHp + "/" + player.Health.MaxHp;
        player.Health.OnHealthChange += UpdateUI;
    }

    private void UpdateUI()
    {
        _text.text = player.Health.CurrentHp + "/" + player.Health.MaxHp;
    }
}
