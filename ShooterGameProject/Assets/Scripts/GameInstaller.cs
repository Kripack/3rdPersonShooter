using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyList enemyList;
    [SerializeField] private PlayerController player;
    public override void InstallBindings()
    {
        BindEnemyList();
        BindPlayerController();
    }

    private void BindPlayerController()
    {
        Container.Bind<PlayerController>().
            FromInstance(player).
            AsSingle();
    }

    private void BindEnemyList()
    {
        Container.Bind<EnemyList>().
            FromInstance(enemyList).
            AsSingle();
    }
}