using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Crosshair : MonoBehaviour
{
    [FormerlySerializedAs("combatSystemController")] [SerializeField] private PlayerCombatController playerCombatController;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float baseAttackFactor;

    private InputReader _input;
    private Vector3 _startScale;
    private Vector3 _targetScale;
    
    [SerializeField] private float walkFactor = 1.2f;
    [SerializeField] private float sprintFactor = 1.5f;
    [SerializeField] private float crouchFactor = 0.75f;
    private float _currentMoveFactor = 1f;
    
    private void Start()
    {
        _input = playerCombatController.input;
        
        _startScale = transform.localScale;
        _targetScale = _startScale;
    }

    private void FixedUpdate()
    {
        if (_input.moveInput != Vector2.zero)
        {
            _currentMoveFactor = walkFactor;
            if (_input.sprint) _currentMoveFactor = sprintFactor;
        }
        else _currentMoveFactor = 1f;

        if (_input.crouch)
            _currentMoveFactor = _input.moveInput == Vector2.zero ? crouchFactor : crouchFactor * walkFactor;
        else if (_input.moveInput == Vector2.zero) _currentMoveFactor = 1f;

        _targetScale = _startScale * _currentMoveFactor;
        
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, returnSpeed * Time.fixedDeltaTime);
    }

    private void ReactOnAttack(WeaponData data)
    {
        transform.DOScale(_startScale * (baseAttackFactor * data.crosshairFactor * _currentMoveFactor), 0.1f)
            .SetEase(Ease.Flash);
    }

    private void OnEnable()
    {
        playerCombatController.OnAttack += ReactOnAttack;
    }

    private void OnDisable()
    {
        playerCombatController.OnAttack -= ReactOnAttack;
    }
}
