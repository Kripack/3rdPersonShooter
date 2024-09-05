public class MeleeWeapon : Weapon
{
    public override void Attack()
    {
        characterAnimator.PlayTargetActionAnimation("MeleeAttack_OneHanded", true);
    }
}
