using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    public event Action Die;
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

        if (_currentHealth <= 0)
        {
            Die?.Invoke();
        }
    }
}
