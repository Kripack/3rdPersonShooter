using UnityEngine;

public interface IAttackStrategy
{
    RaycastHit Execute(Vector3 origin, Vector3 direction, float maxDistance);
}