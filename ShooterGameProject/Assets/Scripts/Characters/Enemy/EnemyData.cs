using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHp;
    public float speed;
    public float chaseSpeedFactor;
    public float rotationSpeed = 6f;
    public float WanderSpeed => speed;
    public float ChaseSpeed => speed * chaseSpeedFactor;
    public float damage;
    public float attackRate;
    public float attackRange;
    public float attackArea;
    public float viewRadius;
    public float innerViewRadius;
    public float viewAngle;
    public float wanderRadius;
 
}
