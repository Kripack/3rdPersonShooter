using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] protected ParticleSystem muzzleFlash;

    protected RangedWeaponData RangeData;
    protected Camera FPSCam;
    public override Weapon Initialize(WeaponData data, CombatSystem combatSystem)
    {
        base.Initialize(data, combatSystem);

        FPSCam = Camera.main;
        CharacterAnimator.Animator.SetBool(CharacterAnimator.AimingBool, true);

        if (data is RangedWeaponData)
        {
            RangeData = (RangedWeaponData) data;
        }
        else { Debug.LogError("This is a Ranged Weapon. The data file should be RangeWeaponData."); }

        return this;
    }

    public override void Disable()
    {
        base.Disable();
        CharacterAnimator.Animator.SetBool(CharacterAnimator.AimingBool, false);
    }

}
