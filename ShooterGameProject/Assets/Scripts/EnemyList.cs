using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public event Action NoEnemies;
    public List<Enemy> Enemies { get; private set; }

    private void Awake()
    {
        Enemies = new List<Enemy>();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }
    
    public void EnemyDefeated(Enemy enemy)
    {
        Enemies.Remove(enemy);

        if (Enemies.Count == 0)
        {
            NoEnemies?.Invoke();
        }
    }
}
