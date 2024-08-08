using System;
public class Health
{
    public event Action Die;
    public event Action<float> OnHealthChange;
    
    private float _maxHealth;
    private float _currentHealth;

    public Health(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_currentHealth <= 0f) return;
        
        _currentHealth -= amount;
        
        OnHealthChange?.Invoke(amount);
        
        if (_currentHealth <= 0)
        {
            Die?.Invoke();
        }
    }
}
