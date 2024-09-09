using UnityEngine;
using Zenject;

public class WinScreen : MonoBehaviour
{
    [Inject] private EnemyList _enemyList;
    [SerializeField] private GameObject holder;

    private void OnEnable()
    {
        _enemyList.NoEnemies += Victory;
    }

    private void Victory()
    {
        holder.SetActive(true);
    }

    private void OnDisable()
    {
        _enemyList.NoEnemies -= Victory;
    }
}