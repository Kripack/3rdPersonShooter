using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform leftHandIKTarget;
    [SerializeField] private Transform leftHandHint;
    
    protected CharacterAnimator CharacterAnimator;
    protected CombatSystem CombatSystem;
    protected AudioSource WeaponAudioSource;

    protected bool IsReloading;
    public virtual Weapon Initialize(WeaponData data, CombatSystem cSystem)
    {
        CombatSystem = cSystem;
        CharacterAnimator = cSystem.CharacterAnimator;
        WeaponAudioSource = GetComponent<AudioSource>();

        CharacterAnimator.SetLeftHandIKTarget(leftHandIKTarget, leftHandHint);

        CharacterAnimator.SetAimRigWeight(1f);
        CharacterAnimator.SetHoldWeaponRigWeight(1f);
        
        return this;
    }
    public virtual void Attack(float damageMultiplier)
    {
        return;
    }
    public virtual void ApplyReload()
    {
        return;
    }
    public virtual void Disable()
    {
        CharacterAnimator.SetAimRigWeight(0f);
        CharacterAnimator.SetHoldWeaponRigWeight(0f);
        
        CharacterAnimator.Animator.CrossFade("Empty", 0.1f, 3);
        
        CharacterAnimator.SetLeftHandIKTarget(null, null);
    }
    protected void CreateImpact(GameObject impactObject ,Vector3 spawnPosition, Quaternion rotation)
    {
        Instantiate(impactObject, spawnPosition, rotation);
    }

}
