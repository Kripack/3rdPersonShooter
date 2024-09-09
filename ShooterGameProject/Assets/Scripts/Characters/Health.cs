using System;
public class Health
{
    public event Action Die;
    public event Action OnHealthDecrease;
    public event Action OnHealthIncrease;
    public float CurrentHp { get; private set; }
    public float MaxHp { get; }

    public Health(float maxHealth)
    {
        MaxHp = maxHealth;
        CurrentHp = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (amount > CurrentHp) CurrentHp = 0f;
        else CurrentHp -= amount;
        
        OnHealthDecrease?.Invoke();
        
        if (CurrentHp <= 0f)
        {
            Die?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;
        
        if (amount >= MaxHp) CurrentHp = MaxHp;
        else CurrentHp += amount;

        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        
        OnHealthIncrease?.Invoke();
    }
}
