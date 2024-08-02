using UnityEngine;

public class ConeDetectionStrategy : IDetectionStrategy 
{
    private readonly float _detectionAngle;
    private readonly float _detectionRadius;
    private readonly float _innerDetectionRadius;
        
    public ConeDetectionStrategy(float detectionRadius = 10f, float innerDetectionRadius = 5f, float detectionAngle = 60f) 
    {
        _detectionAngle = detectionAngle;
        _detectionRadius = detectionRadius;
        _innerDetectionRadius = innerDetectionRadius;
    }
        
    public bool Execute(Transform player, Transform detector) 
    {
        Debug.Log("Player detection call."); // debug
            
        var directionToPlayer = player.position - detector.position;
        var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);
        
        if ((!(angleToPlayer < _detectionAngle / 2f) || !(directionToPlayer.magnitude < _detectionRadius))
            && !(directionToPlayer.magnitude < _innerDetectionRadius)) 
            return false;

        return true;
    }
}