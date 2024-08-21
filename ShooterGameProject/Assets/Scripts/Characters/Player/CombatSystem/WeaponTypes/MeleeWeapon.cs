using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeelleWeapon : Weapon
{
    public override void Attack()
    {
        characterAnimator.PlayTargetActionAnimation("MeleeAttack_OneHanded", false);
    }
}
