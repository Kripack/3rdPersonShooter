using UnityEngine;

public class PlayerDetector
{
    public Transform Player { get; private set; }
    public Health PlayerHealth { get; private set; }
    public Transform DetectorTransform { get; private set; }
    private IDetectionStrategy _detectionStrategy;
    private readonly float _attackRange;
    
    public PlayerDetector(Transform player, Transform detectorTransform, EnemyData data)
    {
        Player = player;
        DetectorTransform = detectorTransform;
        PlayerHealth = player.GetComponent<PlayerController>().Health;
        _attackRange = data.attackRange;
        _detectionStrategy = new ConeDetectionStrategy(data.viewRadius, data.innerViewRadius, data.viewAngle);
    }
    
    public bool CanDetectPlayer()
    {
        return _detectionStrategy.Execute(Player, DetectorTransform);
    }

    public bool CanAttackPlayer() 
    {
        var directionToPlayer = Player.position - DetectorTransform.position;
        return directionToPlayer.magnitude <= _attackRange;
    }
        
    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => _detectionStrategy = detectionStrategy;
    
}