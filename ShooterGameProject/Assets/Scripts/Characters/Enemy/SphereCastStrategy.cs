using UnityEngine;

public class SphereCastStrategy : IAttackStrategy
{
    private readonly float _sphereRadius;
    private readonly LayerMask _layerMask;

    public SphereCastStrategy(LayerMask layerMask, float sphereRadius = 0.1f)
    {
        _sphereRadius = sphereRadius;
        _layerMask = layerMask;
    }
    
    public RaycastHit Execute(Vector3 origin, Vector3 direction, float maxDistance)
    {
        if (Physics.SphereCast(origin, _sphereRadius, direction, out var hit, maxDistance, _layerMask))
        {
            return hit;
        }

        return default;
    }
}