public interface IDamageable
{
    bool IsDead { get; set; }
    void TakeDamage(float amount);
}
