using UnityEngine;

public interface IAttackStrategy
{
    Collider Execute(Vector3 origin, Vector3 direction, float maxDistance);
}