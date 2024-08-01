using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeelleWeapon : Weapon
{
    public override void Attack(float damageMultiplier)
    {
        CombatSystemController.BanAttack();
        CharacterAnimator.PlayTargetActionAnimation("MeleeAttack_OneHanded", false);
    }
}
