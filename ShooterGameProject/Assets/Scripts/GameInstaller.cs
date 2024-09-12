using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyList enemyList;
    [SerializeField] private PlayerController player;
    [FormerlySerializedAs("combatSystemController")] [SerializeField] private PlayerCombatController playerCombatController;
    public override void InstallBindings()
    {
        BindEnemyList();
        BindPlayerController();
        BindCombatSystemController();
        BindCollectiblesInvoker();
    }

    private void BindPlayerController()
    {
        Container.Bind<PlayerController>().
            FromInstance(player).
            AsSingle();
    }

    private void BindCombatSystemController()
    {
        Container.Bind<PlayerCombatController>().
            FromInstance(playerCombatController).
            AsSingle();
    }
    
    private void BindEnemyList()
    {
        Container.Bind<EnemyList>().
            FromInstance(enemyList).
            AsSingle();
    }
    
    private void BindCollectiblesInvoker()
    {
        Container.Bind<CollectiblesInvoker>().
            FromNew().
            AsSingle();
    }
}