using System;
public class Health
{
    public event Action Die;
    public event Action OnHealthChange;
    public float CurrentHp { get; private set; }
    public float MaxHp { get; }

    public Health(float maxHealth)
    {
        MaxHp = maxHealth;
        CurrentHp = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHp <= 0f) return;
        
        CurrentHp -= amount;
        
        OnHealthChange?.Invoke();
        
        if (CurrentHp <= 0)
        {
            Die?.Invoke();
        }
    }
}
