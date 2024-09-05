using UnityEngine;

public class PlayerDetector
{
    public Transform Player { get; private set; }
    public Health PlayerHealth { get; private set; }
    public Transform Origin { get; private set; }
    private IDetectionStrategy _detectionStrategy;
    private readonly float _attackRange;
    
    public PlayerDetector(Transform player, Transform origin, EnemyData data)
    {
        Player = player;
        Origin = origin;
        PlayerHealth = player.GetComponent<PlayerController>().Health;
        _attackRange = data.attackRange;
        _detectionStrategy = new ConeDetectionStrategy(data.viewRadius, data.innerViewRadius, data.viewAngle);
    }
    
    public bool CanDetectPlayer()
    {
        return _detectionStrategy.Execute(Player, Origin);
    }

    public bool CanAttackPlayer() 
    {
        var directionToPlayer = Player.position - Origin.position;
        return directionToPlayer.magnitude <= _attackRange;
    }
    public bool DoesDirectionMatch(float angleThreshold = 1f)
    {
        var directionToPlayer = (Player.position - Origin.position).normalized;
        var angle = Vector3.Angle(Origin.forward, directionToPlayer);
        
        if (angle <= angleThreshold)
        {
            return true;
        }

        return false;
    }    
    
    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => _detectionStrategy = detectionStrategy;
}